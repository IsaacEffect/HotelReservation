namespace HotelReservation.Application.Dtos
{
    public record ObtenerCategoriaDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}