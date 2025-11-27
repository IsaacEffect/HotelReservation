namespace HotelReservation.Domain.Entities
{
    public class Factura
    {
        public Guid Id { get; set; }
        public Guid ReservaId { get; set; }
        public DateTime FechaEmision { get; set; } = DateTime.UtcNow;
        public decimal MontoTotal { get; set; }
        public string MetodoPago { get; set; } = "Efectivo";

        public ICollection<DetalleFactura> Detalles { get; set; } = new List<DetalleFactura>();
    }
}