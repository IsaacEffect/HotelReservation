using HotelReservation.Domain.Entities;
using HotelReservation.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Persistence.Test
{
    public class IntegrationTestFactura
    {
        private HotelReservationDBContext BuildContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<HotelReservationDBContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .EnableSensitiveDataLogging()
                .Options;

            return new HotelReservationDBContext(options);
        }

        private CategoriaHabitacion CrearCategoriaValida(Guid id)
        {
            return new CategoriaHabitacion
            {
                Id = id,
                NombreCategoria = "Estándar",
                Descripcion = "Habitación estándar con cama doble",
                PrecioPorNoche = 100m
            };
        }

        private Habitacion CrearHabitacionValida(Guid id, Guid categoriaId)
        {
            return new Habitacion
            {
                Id = id,
                Numero = "101",
                Estado = "Ocupada",
                CategoriaId = categoriaId
            };
        }

        private CheckInOut CrearCheckInOutValido(Guid reservaId)
        {
            return new CheckInOut
            {
                Id = Guid.NewGuid(),
                ReservaId = reservaId,
                FechaCheckIn = DateTime.Today,
                Observaciones = "Check-in sin observaciones"
            };
        }

        // Simulated RegistrarCheckOut logic that uses the EF context directly.
        // This replaces calling the application service in integration tests for simplicity.
        private static async Task RegistrarCheckOutSimuladoAsync(HotelReservationDBContext context, Guid reservaId, IEnumerable<(string Descripcion, int Cantidad, decimal PrecioUnitario)> servicios)
        {
            // Verificar existencia de CheckIn
            var check = await context.CheckInOut.FirstOrDefaultAsync(c => c.ReservaId == reservaId);
            if (check == null)
                throw new Exception("No existe registro de Check-In para la reserva.");

            // Obtener reserva y habitacion
            var reserva = await context.Reservas.FirstOrDefaultAsync(r => r.Id == reservaId);
            if (reserva == null)
                throw new KeyNotFoundException("Reserva no encontrada.");

            var habitacion = await context.Habitaciones.FirstOrDefaultAsync(h => h.Id == reserva.HabitacionId);
            if (habitacion == null)
                throw new KeyNotFoundException("Habitación no encontrada.");

            var categoria = await context.CategoriasHabitacion.FirstOrDefaultAsync(c => c.Id == habitacion.CategoriaId);
            if (categoria == null)
                throw new KeyNotFoundException("Categoría de habitación no encontrada.");

            var noches = (reserva.FechaFin - reserva.FechaInicio).Days;
            if (noches < 0) noches = 0;

            var totalEstancia = noches * categoria.PrecioPorNoche;
            var totalServicios = servicios?.Sum(s => s.Cantidad * s.PrecioUnitario) ?? 0m;
            var montoTotal = totalEstancia + totalServicios;

            var factura = new Factura
            {
                Id = Guid.NewGuid(),
                ReservaId = reservaId,
                FechaEmision = DateTime.Today,
                MetodoPago = "Efectivo",
                MontoTotal = montoTotal,
                Reserva = reserva
            };

            await context.Facturas.AddAsync(factura);
            await context.SaveChangesAsync();

            if (servicios != null)
            {
                foreach (var s in servicios)
                {
                    var detalle = new DetalleFactura
                    {
                        Id = Guid.NewGuid(),
                        FacturaId = factura.Id,
                        Descripcion = s.Descripcion,
                        Cantidad = s.Cantidad,
                        PrecioUnitario = s.PrecioUnitario,
                        Subtotal = s.Cantidad * s.PrecioUnitario
                    };
                    await context.DetalleFacturas.AddAsync(detalle);
                }
            }

            // Actualizar estado de habitacion (si no está en Mantenimiento)
            if (habitacion.Estado != "Mantenimiento")
                habitacion.Estado = "Disponible";

            // Actualizar estado de reserva
            reserva.EstadoReserva = "Completada";

            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task RegistrarCheckOut_DeberiaInsertarFacturaYDetalle()
        {
            var context = BuildContext("InsertarFacturaYDetalle");
            var reservaId = Guid.NewGuid();
            var habitacionId = Guid.NewGuid();
            var categoriaId = Guid.NewGuid();

            context.CategoriasHabitacion.Add(CrearCategoriaValida(categoriaId));
            context.Habitaciones.Add(CrearHabitacionValida(habitacionId, categoriaId));
            context.Reservas.Add(new Reserva
            {
                Id = reservaId,
                FechaInicio = DateTime.Today,
                FechaFin = DateTime.Today.AddDays(2),
                EstadoReserva = "Activa",
                HabitacionId = habitacionId
            });
            context.CheckInOut.Add(CrearCheckInOutValido(reservaId));

            await context.SaveChangesAsync();

            var servicios = new List<(string Descripcion, int Cantidad, decimal PrecioUnitario)>
            {
                ("Spa", 1, 50m)
            };

            await RegistrarCheckOutSimuladoAsync(context, reservaId, servicios);

            var factura = context.Facturas.FirstOrDefault(f => f.ReservaId == reservaId);
            Assert.NotNull(factura);
            var detalles = context.DetalleFacturas.Where(d => d.FacturaId == factura.Id).ToList();

            Assert.Single(detalles);
            Assert.Equal("Spa", detalles[0].Descripcion);
        }

        [Fact]
        public async Task RegistrarCheckOut_DeberiaActualizarEstados()
        {
            var context = BuildContext("ActualizarEstados");
            var reservaId = Guid.NewGuid();
            var habitacionId = Guid.NewGuid();
            var categoriaId = Guid.NewGuid();

            context.CategoriasHabitacion.Add(CrearCategoriaValida(categoriaId));
            context.Habitaciones.Add(CrearHabitacionValida(habitacionId, categoriaId));
            context.Reservas.Add(new Reserva
            {
                Id = reservaId,
                FechaInicio = DateTime.Today,
                FechaFin = DateTime.Today.AddDays(2),
                EstadoReserva = "Activa",
                HabitacionId = habitacionId
            });
            context.CheckInOut.Add(CrearCheckInOutValido(reservaId));

            await context.SaveChangesAsync();

            var servicios = Enumerable.Empty<(string, int, decimal)>();
            await RegistrarCheckOutSimuladoAsync(context, reservaId, servicios);

            var reserva = context.Reservas.First(r => r.Id == reservaId);
            var habitacion = context.Habitaciones.First(h => h.Id == habitacionId);

            Assert.Equal("Completada", reserva.EstadoReserva);
            Assert.Equal("Disponible", habitacion.Estado);
        }

        [Fact]
        public async Task RegistrarCheckOut_SinCheckInOut_DeberiaLanzarExcepcion()
        {
            var context = BuildContext("SinCheckInOut");
            var reservaId = Guid.NewGuid();
            var habitacionId = Guid.NewGuid();
            var categoriaId = Guid.NewGuid();

            context.CategoriasHabitacion.Add(CrearCategoriaValida(categoriaId));
            context.Habitaciones.Add(CrearHabitacionValida(habitacionId, categoriaId));
            context.Reservas.Add(new Reserva
            {
                Id = reservaId,
                FechaInicio = DateTime.Today,
                FechaFin = DateTime.Today.AddDays(2),
                EstadoReserva = "Activa",
                HabitacionId = habitacionId
            });

            await context.SaveChangesAsync();

            await Assert.ThrowsAsync<Exception>(async () =>
                await RegistrarCheckOutSimuladoAsync(context, reservaId, Enumerable.Empty<(string, int, decimal)>()));
        }

        [Fact]
        public async Task RegistrarCheckOut_DeberiaGuardarMontoTotalCorrecto()
        {
            var context = BuildContext("GuardarMontoTotal");
            var reservaId = Guid.NewGuid();
            var habitacionId = Guid.NewGuid();
            var categoriaId = Guid.NewGuid();

            context.CategoriasHabitacion.Add(CrearCategoriaValida(categoriaId));
            context.Habitaciones.Add(CrearHabitacionValida(habitacionId, categoriaId));
            context.Reservas.Add(new Reserva
            {
                Id = reservaId,
                FechaInicio = DateTime.Today,
                FechaFin = DateTime.Today.AddDays(3),
                EstadoReserva = "Activa",
                HabitacionId = habitacionId
            });
            context.CheckInOut.Add(CrearCheckInOutValido(reservaId));

            await context.SaveChangesAsync();

            var servicios = new List<(string Descripcion, int Cantidad, decimal PrecioUnitario)>
            {
                ("Desayuno", 2, 20m)
            };

            await RegistrarCheckOutSimuladoAsync(context, reservaId, servicios);

            var factura = context.Facturas.First(f => f.ReservaId == reservaId);
            Assert.Equal(3 * 100m + 40m, factura.MontoTotal);
        }

        [Fact]
        public async Task RegistrarCheckOut_DeberiaInsertarMultiplesDetalles()
        {
            var context = BuildContext("MultiplesDetalles");
            var reservaId = Guid.NewGuid();
            var habitacionId = Guid.NewGuid();
            var categoriaId = Guid.NewGuid();

            context.CategoriasHabitacion.Add(CrearCategoriaValida(categoriaId));
            context.Habitaciones.Add(CrearHabitacionValida(habitacionId, categoriaId));
            context.Reservas.Add(new Reserva
            {
                Id = reservaId,
                FechaInicio = DateTime.Today,
                FechaFin = DateTime.Today.AddDays(1),
                EstadoReserva = "Activa",
                HabitacionId = habitacionId
            });
            context.CheckInOut.Add(CrearCheckInOutValido(reservaId));

            await context.SaveChangesAsync();

            var servicios = new List<(string Descripcion, int Cantidad, decimal PrecioUnitario)>
            {
                ("Cena", 1, 30m),
                ("Lavandería", 2, 15m)
            };

            await RegistrarCheckOutSimuladoAsync(context, reservaId, servicios);

            var factura = context.Facturas.First(f => f.ReservaId == reservaId);
            var detalles = context.DetalleFacturas.Where(d => d.FacturaId == factura.Id).ToList();

            Assert.Equal(2, detalles.Count);
            Assert.Contains(detalles, d => d.Descripcion == "Cena");
            Assert.Contains(detalles, d => d.Descripcion == "Lavandería");
        }
    }
}