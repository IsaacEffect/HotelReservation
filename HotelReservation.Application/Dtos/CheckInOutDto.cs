namespace HotelReservation.Application.Dtos
{
    public class CheckInOutDto
    {
        public Guid Id { get; set; }
        public Guid ReservaId { get; set; }
        public DateTime? FechaCheckIn { get; set; }
        public DateTime? FechaCheckOut { get; set; }
        public string? Observaciones { get; set; }
    }

}