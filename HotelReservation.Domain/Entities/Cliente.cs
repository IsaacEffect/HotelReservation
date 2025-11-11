namespace HotelReservation.Domain.Entities
{
    public class Cliente
    {
        public Guid IdCliente { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public string? DocumentoIdentidad { get; set; }
        public bool Estado { get; set; } = true;

        //public virtual ICollection<Reserva>? Reservas { get; set; }
        //public virtual ICollection<HistorialReserva>? HistorialReservas { get; set; }
    }
}
