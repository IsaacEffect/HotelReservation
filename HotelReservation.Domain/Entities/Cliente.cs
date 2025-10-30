using System;
using System.Collections.Generic;

namespace HotelReservation.Domain.Entities
{
    public class Cliente
    {
        public Guid Id { get; set; } 
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public string? DocumentoIdentidad { get; set; }
        public DateTime FechaCreacion { get; set; }

        public virtual ICollection<Reserva>? Reservas { get; set; }
    }
}
