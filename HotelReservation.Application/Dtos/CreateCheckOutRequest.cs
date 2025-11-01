namespace HotelReservation.Application.DTOs
{
    public class CreateCheckOutRequest
    {
        public Guid ReservaId { get; set; }
        public DateTime FechaCheckOut { get; set; } = DateTime.UtcNow;
        public string? Observaciones { get; set; }
    }

}