using System;
using System.Collections.Generic;

namespace HotelReservation.Domain.Entities
{
    public class CategoriaHabitacion
    {
        public Guid Id { get; set; }
        public string NombreCategoria { get; set; }
        public string Descripcion { get; set; }
        public int Capacidad { get; set; }
        public decimal PrecioPorNoche { get; set; }

        public ICollection<Habitacion> Habitaciones { get; set; }
    }
}