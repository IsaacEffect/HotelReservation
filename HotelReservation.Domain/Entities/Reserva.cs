namespace HotelReservation.Domain.Entities
{
    public class Reserva
    {
        public Guid Id { get; set; }
        public DateTime FechaReserva { get; set; } = DateTime.UtcNow;
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string EstadoReserva { get; set; } = "Pendiente";
        public Guid ClienteId { get; set; }
        public Guid HabitacionId { get; set; }
        public Guid UsuarioId { get; set; }
        public decimal Total { get; set; }
    }

}