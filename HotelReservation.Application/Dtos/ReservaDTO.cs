namespace HotelReservation.Application.Dtos
{
    public class ReservaDTO
    {
        public Guid Id { get; set; }
        public DateTime FechaReserva { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string? EstadoReserva { get; set; }
        public Guid ClienteId { get; set; }
        public Guid HabitacionId { get; set; }
        public Guid UsuarioId { get; set; }
        public decimal? Total { get; set; }
    }
}