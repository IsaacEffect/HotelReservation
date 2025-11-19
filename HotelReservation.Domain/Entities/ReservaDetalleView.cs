using System;

namespace HotelReservation.Domain.Entities
{
    public class ReservasDetalleView
    {
        public Guid ReservaId { get; set; }
        public DateTime FechaReserva { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string EstadoReserva { get; set; }
        public string Cliente { get; set; }
        public string CorreoCliente { get; set; }
        public string NumeroHabitacion { get; set; }
        public string EstadoHabitacion { get; set; }
        public string Categoria { get; set; }
        public decimal PrecioPorNoche { get; set; }
        public string UsuarioRegistro { get; set; }
        public decimal? Total { get; set; }
    }
}
