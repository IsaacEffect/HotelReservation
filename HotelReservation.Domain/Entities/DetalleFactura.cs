using System;

namespace HotelReservation.Domain.Entities
{
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