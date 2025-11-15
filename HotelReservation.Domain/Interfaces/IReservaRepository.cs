using HotelReservation.Domain.Entities;


namespace HotelReservation.Domain.Interfaces
{
    public interface IReservaRepository
    {
        Task<Reserva?> GetByIdAsync(Guid id);
        Task<IEnumerable<Reserva>> GetAllAsync();
        Task AddAsync(Reserva entity);
        Task UpdateAsync(Reserva entity);
        Task DeleteAsync(Guid id);

        Task<bool> HabitacionDisponibleAsync(Guid habitacionId, DateTime fechaInicio, DateTime fechaFin);
        Task<IEnumerable<object>> ObtenerReservasConDetallesAsync();
        Task<IEnumerable<Reserva>> GetByEstadoAsync(string estado);
    }
}