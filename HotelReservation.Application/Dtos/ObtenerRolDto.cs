namespace HotelReservation.Application.Dtos
{
    public record ObtenerRolDto
    {
        public Guid RolId { get; set; }
        public string NombreRol { get; set; } = string.Empty;
    }
}
