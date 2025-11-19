namespace HotelReservation.Domain.Entities
{
    public class CategoriaHabitacion
    {
        public Guid Id { get; set; }
        public string NombreCategoria { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public int Capacidad { get; set; }
        public decimal PrecioPorNoche { get; set; }
    }
}