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
    }
}
