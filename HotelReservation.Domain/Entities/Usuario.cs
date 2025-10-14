namespace HotelReservation.Domain.Entities
{
    public class Usuario
    {
        public Guid IdUsuario { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Contraseña { get; set; } = string.Empty;

        public Guid IdRol { get; set; }
        public virtual Rol? Rol { get; set; }

        public virtual ICollection<Reserva>? Reservas { get; set; }
    }
}
