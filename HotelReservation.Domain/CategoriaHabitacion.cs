using System;

namespace HotelReservation.Domain
{
      public class CategoriaHabitacion
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }
        public decimal PrecioBase { get; set; }
    }
}