namespace HotelReservation.Application.Dtos

{
    public class ActualizarReservaDTO
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string EstadoReserva { get; set; } = "Pendiente";
    }
}