using System;

namespace HotelReservation.Application.DTOs
{
    public class CreateCheckInRequest
    {
        public Guid ReservaId { get; set; }
        public DateTime FechaCheckIn { get; set; } = DateTime.UtcNow;
        public string? Observaciones { get; set; }
    }

}
