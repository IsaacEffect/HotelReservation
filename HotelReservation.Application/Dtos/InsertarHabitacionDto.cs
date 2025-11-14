namespace HotelReservation.Application.Dtos
{
    public class InsertarHabitacionDto
    {
        public string Numero { get; set; } = string.Empty;
        public Guid CategoriaId { get; set; }
    }
}
