using System;

namespace HotelReservation.Domain.Entities
{
    public class DetalleReserva
    {
        public Guid Id { get; set; }
        public Guid ReservaId { get; set; }
        public string Descripcion { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }

        public Reserva Reserva { get; set; }
    }
}