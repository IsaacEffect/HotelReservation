using System;
using System.Collections.Generic;

namespace HotelReservation.Domain.Entities
{
    public class Usuario
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Correo { get; set; }
        public string Contraseña { get; set; }
        public Guid RolId { get; set; }
        public DateTime FechaCreacion { get; set; }

        public Rol Rol { get; set; }
        public ICollection<Reserva> Reservas { get; set; }
    }
}
