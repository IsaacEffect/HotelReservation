using HotelReservation.Domain.Entities;

namespace HotelReservation.Domain.Interfaces
{
    public interface IReservaRepository
    {
        Task<Reserva?> GetByIdAsync(Guid id);
        Task<IEnumerable<Reserva>> GetAllAsync();
        Task<Reserva> AddAsync(Reserva entity);
        Task<Reserva> UpdateAsync(Reserva entity);
        Task DeleteAsync(Guid id);

        // --------------------------------------------------
        Task<bool> HabitacionDisponibleAsync(Guid habitacionId, DateTime fechaInicio, DateTime fechaFin);
        Task<Guid> CrearReservaAsync(Reserva reserva);
        Task<IEnumerable<Reserva>> ObtenerReservasAsync();
        Task<IEnumerable<object>> ObtenerReservasConDetallesAsync();
    }
}