using System;
using System.Collections.Generic;

namespace HotelReservation.Domain.Entities
{
    public class Habitacion
    {
        public Guid Id { get; set; }
        public string Numero { get; set; }
        public string Estado { get; set; }
        public Guid CategoriaId { get; set; }

        public CategoriaHabitacion Categoria { get; set; }
        public ICollection<Reserva> Reservas { get; set; }
        public ICollection<HistorialReserva> HistorialReservas { get; set; }
    }
}