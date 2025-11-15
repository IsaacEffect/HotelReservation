namespace HotelReservation.Application.Dtos
{
    public class DetalleFacturaDto
    {
        public Guid Id { get; set; }
        public Guid FacturaId { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
    }

    public class FacturaDto
    {
        public Guid Id { get; set; }
        public Guid ReservaId { get; set; }
        public DateTime FechaEmision { get; set; }
        public decimal MontoTotal { get; set; }
        public string MetodoPago { get; set; } = string.Empty;

        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public string HuespedNombre { get; set; } = string.Empty;

        public IEnumerable<DetalleFacturaDto> Detalles { get; set; } = new List<DetalleFacturaDto>();
    }
}