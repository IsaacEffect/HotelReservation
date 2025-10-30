using System;
using System.Collections.Generic;

namespace HotelReservation.Domain.Entities
{
    public class Habitacion
    {
        public Guid Id { get; set; } 
        public string Numero { get; set; } = string.Empty;
        public string Estado { get; set; } = "Disponible";
        public Guid CategoriaId { get; set; }

        public virtual ICollection<Reserva>? Reservas { get; set; }
    }
}
