namespace HotelReservation.Domain.Entities
{
    public class Rol
    {
        public Guid IdRol { get; set; }
        public string NombreRol { get; set; } = string.Empty;

        public virtual ICollection<Usuario>? Usuarios { get; set; }
    }
}
