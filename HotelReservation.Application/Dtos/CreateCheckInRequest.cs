<<<<<<< HEAD
﻿namespace HotelReservation.Application.DTOs
=======
﻿namespace HotelReservation.Application.Dtos
>>>>>>> origin/develop
{
    public class CreateCheckInRequest
    {
        public Guid ReservaId { get; set; }
        public DateTime FechaCheckIn { get; set; } = DateTime.UtcNow;
        public string? Observaciones { get; set; }
    }

}