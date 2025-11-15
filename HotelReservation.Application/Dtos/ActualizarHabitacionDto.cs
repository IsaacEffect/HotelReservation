namespace HotelReservation.Application.Dtos
{
    public class ActualizarHabitacionDto
    {
        public string Numero { get; set; } = string.Empty;
        public Guid CategoriaId { get; set; }
    }
}
