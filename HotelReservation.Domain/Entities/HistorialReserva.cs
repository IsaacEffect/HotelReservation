using System;

namespace HotelReservation.Domain.Entities
{
    public class HistorialReserva
    {
        public Guid Id { get; set; } 
        public Guid HabitacionId { get; set; }
        public Guid ClienteId { get; set; }
        public DateTime FechaEntrada { get; set; }
        public DateTime FechaSalida { get; set; }
        public string? Motivo { get; set; }
    }
}
