namespace HotelReservation.Domain.Entities
{
    public class CheckInOut
    {
        public Guid Id { get; set; }
        public Guid ReservaId { get; set; }
        public DateTime? FechaCheckIn { get; set; }
        public DateTime? FechaCheckOut { get; set; }
        public string? Observaciones { get; set; }

        public virtual Reserva? Reserva { get; set; }
    }
}