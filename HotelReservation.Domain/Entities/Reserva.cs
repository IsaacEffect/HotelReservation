using System;
using System.Collections.Generic;

namespace HotelReservation.Domain.Entities
{
    public class Reserva
    {
        public Guid Id { get; set; }
        public DateTime FechaReserva { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string EstadoReserva { get; set; }
        public Guid ClienteId { get; set; }
        public Guid HabitacionId { get; set; }
        public Guid UsuarioId { get; set; }
        public decimal? Total { get; set; }

        public Cliente Cliente { get; set; }
        public Habitacion Habitacion { get; set; }
        public Usuario Usuario { get; set; }
        public ICollection<DetalleReserva> Detalles { get; set; }
        public Factura Factura { get; set; }
        public CheckInOut CheckInOut { get; set; }
    }
}
