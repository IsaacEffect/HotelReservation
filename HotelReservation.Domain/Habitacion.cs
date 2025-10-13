using System;

namespace HotelReservation.Domain
{
       public class Habitacion
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Numero { get; set; } = null!;
        public Guid CategoriaId { get; set; }
        public string Estado { get; set; } = "Disponible"; // Disponible, Ocupada, Mantenimiento
    }
}