namespace HotelReservation.Application.Dtos

{
    public record ActualizarReservaDTO
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
    }
}

