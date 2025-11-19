<<<<<<< HEAD
﻿namespace HotelReservation.Application.DTOs
=======
﻿namespace HotelReservation.Application.Dtos
>>>>>>> origin/develop
{
    public class CreateCheckOutRequest
    {
        public Guid ReservaId { get; set; }
        public DateTime FechaCheckOut { get; set; } = DateTime.UtcNow;
        public string? Observaciones { get; set; }
    }

}