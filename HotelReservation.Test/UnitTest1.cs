using HotelReservation.Domain.Entities;

namespace HotelReservation.Test
{
    public class UnitTest1
    {
        [Fact]
        public void CalcularTotal_DeberiaSumarEstanciaYServicios()
        {
            var reserva = new Reserva
            {
                FechaInicio = new DateTime(2025, 11, 1),
                FechaFin = new DateTime(2025, 11, 5),
                Habitacion = new Habitacion
                {
                    Categoria = new CategoriaHabitacion { PrecioPorNoche = 100 }
                },
                Detalles = new List<DetalleReserva>
        {
            new DetalleReserva { Cantidad = 2, PrecioUnitario = 50 },
            new DetalleReserva { Cantidad = 1, PrecioUnitario = 30 }
        }
            };

            var noches = (reserva.FechaFin - reserva.FechaInicio).Days;
            var totalEstancia = noches * reserva.Habitacion.Categoria.PrecioPorNoche;
            var totalServicios = reserva.Detalles.Sum(d => d.Cantidad * d.PrecioUnitario);
            var totalFinal = totalEstancia + totalServicios;

            Assert.Equal(530m, totalFinal);
        }

        [Fact]
        public void CalcularSubtotal_DeberiaMultiplicarCantidadYPrecio()
        {
            var detalle = new DetalleReserva
            {
                Cantidad = 3,
                PrecioUnitario = 40
            };

            var subtotal = detalle.Cantidad * detalle.PrecioUnitario;

            Assert.Equal(120m, subtotal);
        }

        [Fact]
        public void CalcularNoches_DeberiaRetornarDiferenciaDeDias()
        {
            var inicio = new DateTime(2025, 11, 10);
            var fin = new DateTime(2025, 11, 15);

            var noches = (fin - inicio).Days;

            Assert.Equal(5, noches);
        }

        [Fact]
        public void GenerarFactura_DeberiaAsignarPropiedadesCorrectas()
        {
            var reservaId = Guid.NewGuid();
            var total = 500m;

            var factura = new Factura
            {
                Id = Guid.NewGuid(),
                ReservaId = reservaId,
                FechaEmision = DateTime.Today,
                MetodoPago = "Efectivo",
                MontoTotal = total
            };

            Assert.Equal(reservaId, factura.ReservaId);
            Assert.Equal("Efectivo", factura.MetodoPago);
            Assert.Equal(total, factura.MontoTotal);
        }

        [Fact]
        public void GenerarDetalleFactura_SinAsignarSubtotal_DeberiaSerValido()
        {
            var facturaId = Guid.NewGuid();

            var detalleFactura = new DetalleFactura
            {
                Id = Guid.NewGuid(),
                FacturaId = facturaId,
                Descripcion = "Servicio de Spa",
                Cantidad = 2,
                PrecioUnitario = 75.00m
                // Subtotal se calcula en SQL Server
            };

            Assert.Equal("Servicio de Spa", detalleFactura.Descripcion);
            Assert.Equal(2, detalleFactura.Cantidad);
            Assert.Equal(75.00m, detalleFactura.PrecioUnitario);
        }
    }
}