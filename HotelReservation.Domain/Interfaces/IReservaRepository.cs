using HotelReservation.Domain.Entities;


namespace HotelReservation.Domain.Interfaces
{
    public interface IReservaRepository
    {
        // CRUD principales
        Task<Guid> CrearReservaAsync(Reserva reserva);
        Task<IEnumerable<Reserva>> ObtenerReservasAsync();
        Task<Reserva?> ObtenerReservaPorIdAsync(Guid id);
        Task ModificarReservaAsync(Reserva reserva);
        Task CancelarReservaAsync(Guid id);

        // Disponibilidad de habitación
        Task<bool> HabitacionDisponibleAsync(Guid habitacionId, DateTime fechaInicio, DateTime fechaFin);
        Task<bool> HabitacionDisponibleAsync(Guid habitacionId, DateTime fechaInicio, DateTime fechaFin, Guid? reservaIdExcluir);

        // Consultas avanzadas (JOIN)
        Task<IEnumerable<object>> ObtenerReservasConDetallesAsync();
        Task<decimal> ObtenerPrecioHabitacionAsync(Guid habitacionId);


        // Consultas por estado (si las necesitas)
        Task<IEnumerable<Reserva>> ObtenerReservasPorEstadoAsync(string estado);
    }
}