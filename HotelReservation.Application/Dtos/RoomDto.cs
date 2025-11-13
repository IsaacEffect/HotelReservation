namespace HotelReservation.Application.DTOs
{
    public class RoomDto
    {
        public int Id { get; set; }
        public string Numero { get; set; } = string.Empty;
        public decimal PrecioPorNoche { get; set; }
        public int Capacidad { get; set; }
        public int CategoriaId { get; set; }
        public string Estado { get; set; } = "Disponible";
    }
}