using System;
using System.Collections.Generic;

namespace HotelReservation.Domain.Entities
{
    public class Cliente
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string DocumentoIdentidad { get; set; }
        public DateTime FechaCreacion { get; set; }

        public ICollection<Reserva> Reservas { get; set; }
        public ICollection<HistorialReserva> HistorialReservas { get; set; }
    }
}
