namespace HotelReservation.Application.DTOs
{
    public class OcupacionDiariaDto
    {
        public DateTime Fecha { get; set; } = DateTime.UtcNow;
        public int TotalHabitaciones { get; set; }
        public int Ocupadas { get; set; }
        public int Disponibles { get; set; }
        public int Mantenimiento { get; set; }
        public double PorcentajeOcupacion { get; set; }
    }
}