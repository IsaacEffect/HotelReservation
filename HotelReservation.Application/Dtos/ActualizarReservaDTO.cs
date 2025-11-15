namespace HotelReservation.Application.Dtos

{
    public record ActualizarReservaDTO
    {
        public Guid ReservaId { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
    }
}

