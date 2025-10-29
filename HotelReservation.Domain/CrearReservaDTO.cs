using System;

namespace HotelReservation.Domain

{
     public class CrearReservaDTO
{
      public Guid ClienteId { get; set; }
       public Guid HabitacionId { get; set; }
       public Guid UsuarioId { get; set; }
       public DateTime FechaInicio { get; set; }
       public DateTime FechaFin { get; set; }
       public decimal PrecioPorNoche { get; set; }
}
}