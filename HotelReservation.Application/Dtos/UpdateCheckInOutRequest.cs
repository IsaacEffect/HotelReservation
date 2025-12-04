namespace HotelReservation.Application.Dtos
{
    public class UpdateCheckInOutRequest
    {
        public DateTime? FechaCheckIn { get; set; }
        public DateTime? FechaCheckOut { get; set; }
        public string? Observaciones { get; set; }
    }

}
