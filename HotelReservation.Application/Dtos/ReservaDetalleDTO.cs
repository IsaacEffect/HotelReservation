namespace HotelReservation.Application.Dtos
{
    public record ReservaDetalleDTO
    {
        public Guid ReservaId { get; set; }
        public DateTime FechaReserva { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string? EstadoReserva { get; set; }
        public string? Cliente { get; set; }
        public string? CorreoCliente { get; set; }
        public int NumeroHabitacion { get; set; }
        public string? Categoria { get; set; }
        public decimal PrecioPorNoche { get; set; }
        public string? UsuarioRegistro { get; set; }
        public decimal? Total { get; set; }
    }
}