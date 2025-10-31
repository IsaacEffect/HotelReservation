using System;
using System.Collections.Generic;

namespace HotelReservation.Domain.Entities
{
    public class Rol
    {
        public Guid Id { get; set; }
        public string NombreRol { get; set; }

        public ICollection<Usuario> Usuarios { get; set; }
    }
}
