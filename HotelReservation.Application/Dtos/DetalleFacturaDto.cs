namespace HotelReservation.Application.Dtos
{
    public class DetalleFacturaDto
    {
        public string? Descripcion { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal => Cantidad * PrecioUnitario;
    }
}