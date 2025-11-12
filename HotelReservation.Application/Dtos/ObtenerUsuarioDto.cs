namespace HotelReservation.Application.Dtos
{
    public record ObtenerUsuarioDto
    {
        public Guid IdUsuario { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public bool Estado { get; set; }
        public ObtenerRolDto? Rol { get; set; }
    }
}
