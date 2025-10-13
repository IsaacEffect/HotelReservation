using System;

namespace HotelReservation.Domain
{
    public enum EstadoReserva { Pendiente, Confirmada, Cancelada, Completada }
        
        public class Reserva
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ClienteId { get; set; }
        public Guid HabitacionId { get; set; }
        public DateTime FechaEntrada { get; set; }
        public DateTime FechaSalida { get; set; }
        public EstadoReserva Estado { get; set; } = EstadoReserva.Pendiente;
        public decimal Total { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    }

}