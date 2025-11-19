<<<<<<< HEAD
﻿namespace HotelReservation.Application.DTOs
=======
﻿namespace HotelReservation.Application.Dtos
>>>>>>> origin/develop
{
    public class OcupacionDiariaDto
    {
        public DateTime Fecha { get; set; } = DateTime.UtcNow;
        public int TotalHabitaciones { get; set; }
        public int Ocupadas { get; set; }
        public int Disponibles { get; set; }
        public int Mantenimiento { get; set; }
        public double PorcentajeOcupacion { get; set; }
    }
}