using HotelReservation.Domain.Entities;

namespace HotelReservation.Application.Test
{
    public class UnitTestFactura
    {
        [Fact]
        public void CalcularTotal_DeberiaSumarEstanciaYServicios()
        {
            var fechaInicio = new DateTime(2025, 11, 1);
            var fechaFin = new DateTime(2025, 11, 5);

            var noches = (fechaFin - fechaInicio).Days;
            decimal precioPorNoche = 100m;

            var servicios = new List<(int Cantidad, decimal PrecioUnitario)>
            {
                (2, 50m),
                (1, 30m)
            };

            var totalEstancia = noches * precioPorNoche;
            var totalServicios = servicios.Sum(s => s.Cantidad * s.PrecioUnitario);
            var totalFinal = totalEstancia + totalServicios;

            Assert.Equal(530m, totalFinal);
        }

        [Fact]
        public void CalcularSubtotal_DeberiaMultiplicarCantidadYPrecio()
        {
            var cantidad = 3;
            decimal precioUnitario = 40m;

            var subtotal = cantidad * precioUnitario;

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

            var reserva = new Reserva
            {
                Id = reservaId,
                FechaInicio = DateTime.Today,
                FechaFin = DateTime.Today.AddDays(1),
                ClienteId = Guid.Empty,
                HabitacionId = Guid.Empty,
                UsuarioId = Guid.Empty,
                Total = total
            };

            var factura = new Factura
            {
                Id = Guid.NewGuid(),
                ReservaId = reservaId,
                Reserva = reserva,
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
            };

            Assert.Equal("Servicio de Spa", detalleFactura.Descripcion);
            Assert.Equal(2, detalleFactura.Cantidad);
            Assert.Equal(75.00m, detalleFactura.PrecioUnitario);
        }
    }
}