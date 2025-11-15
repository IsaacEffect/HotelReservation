namespace HotelReservation.Application.Dtos
{
    public class ActualizarEstadoReservaDTO
    {
        public Guid ReservaId { get; set; }
        public string? NuevoEstado { get; set; }
    }
}
