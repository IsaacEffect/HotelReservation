using System.ComponentModel.DataAnnotations.Schema;

namespace HotelReservation.Domain.Entities
{
    [Table("DetalleFactura")]
    public class DetalleFactura
    {
        public Guid Id { get; set; }
        public Guid FacturaId { get; set; }
        public string Descripcion { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }

        public Factura Factura { get; set; }
    }
}