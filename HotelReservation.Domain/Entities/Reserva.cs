namespace HotelReservation.Domain.Entities
{
    public class Reserva
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public DateTime FechaReserva { get; init; } = DateTime.UtcNow;
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string EstadoReserva { get; set; } = "Pendiente";
        public Guid ClienteId { get; set; }
        public Guid HabitacionId { get; set; }
        public Guid UsuarioId { get; set; }
        public decimal? Total { get; private set; }

        // --- Propiedades de Navegación ---
        public virtual Cliente? Cliente { get; set; }
        public virtual Habitacion? Habitacion { get; set; }
        public virtual Usuario? Usuario { get; set; }
    }
}
