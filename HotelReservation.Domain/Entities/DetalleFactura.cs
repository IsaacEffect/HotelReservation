namespace HotelReservation.Domain.Entities
{
    public class DetalleFactura
    {
        public Guid Id { get; set; }
        public Guid FacturaId { get; set; }
        public string? Descripcion { get; set; }
        public int Cantidad { get; set; } = 1;
        public decimal PrecioUnitario { get; set; }

        public decimal Subtotal => Cantidad * PrecioUnitario;
    }
}