using Xunit;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using HotelReservation.Application.Services;
using HotelReservation.Domain.Entities;
using HotelReservation.Persistence.Contexts;

namespace HotelReservation.IntegracionTest
{
    public class UnitTest1
    {
        private HotelReservationContext BuildContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<HotelReservationContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .EnableSensitiveDataLogging()
                .Options;

            return new HotelReservationContext(options);
        }

        private CategoriaHabitacion CrearCategoriaValida(Guid id)
        {
            return new CategoriaHabitacion
            {
                Id = id,
                NombreCategoria = "Estándar",
                Descripcion = "Habitación estándar con cama doble",
                PrecioPorNoche = 100
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
                HabitacionId = habitacionId,
                Detalles = new List<DetalleReserva>
                {
                    new DetalleReserva { Id = Guid.NewGuid(), Descripcion = "Spa", Cantidad = 1, PrecioUnitario = 50 }
                }
            });
            context.CheckInOut.Add(CrearCheckInOutValido(reservaId));

            await context.SaveChangesAsync();

            var service = new CheckInOutService(context);
            await service.RegistrarCheckOutAsync(reservaId);

            var factura = context.Facturas.FirstOrDefault(f => f.ReservaId == reservaId);
            var detalles = context.DetalleFacturas.Where(d => d.FacturaId == factura.Id).ToList();

            Assert.NotNull(factura);
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
                HabitacionId = habitacionId,
                Detalles = new List<DetalleReserva>()
            });
            context.CheckInOut.Add(CrearCheckInOutValido(reservaId));

            await context.SaveChangesAsync();

            var service = new CheckInOutService(context);
            await service.RegistrarCheckOutAsync(reservaId);

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
                HabitacionId = habitacionId,
                Detalles = new List<DetalleReserva>()
            });

            await context.SaveChangesAsync();

            var service = new CheckInOutService(context);

            await Assert.ThrowsAsync<Exception>(() =>
                service.RegistrarCheckOutAsync(reservaId));
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
                HabitacionId = habitacionId,
                Detalles = new List<DetalleReserva>
                {
                    new DetalleReserva { Id = Guid.NewGuid(), Descripcion = "Desayuno", Cantidad = 2, PrecioUnitario = 20 }
                }
            });
            context.CheckInOut.Add(CrearCheckInOutValido(reservaId));

            await context.SaveChangesAsync();

            var service = new CheckInOutService(context);
            await service.RegistrarCheckOutAsync(reservaId);

            var factura = context.Facturas.First(f => f.ReservaId == reservaId);
            Assert.Equal(3 * 100 + 40, factura.MontoTotal);
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
                HabitacionId = habitacionId,
                Detalles = new List<DetalleReserva>
                {
                    new DetalleReserva { Id = Guid.NewGuid(), Descripcion = "Cena", Cantidad = 1, PrecioUnitario = 30 },
                    new DetalleReserva { Id = Guid.NewGuid(), Descripcion = "Lavandería", Cantidad = 2, PrecioUnitario = 15 }
                }
            });
            context.CheckInOut.Add(CrearCheckInOutValido(reservaId));

            await context.SaveChangesAsync();

            var service = new CheckInOutService(context);
            await service.RegistrarCheckOutAsync(reservaId);

            var factura = context.Facturas.First(f => f.ReservaId == reservaId);
            var detalles = context.DetalleFacturas.Where(d => d.FacturaId == factura.Id).ToList();

            Assert.Equal(2, detalles.Count);
            Assert.Contains(detalles, d => d.Descripcion == "Cena");
            Assert.Contains(detalles, d => d.Descripcion == "Lavandería");
        }
    }
}