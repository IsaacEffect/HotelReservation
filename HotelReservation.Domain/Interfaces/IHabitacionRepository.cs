using HotelReservation.Domain.Entities;

namespace HotelReservation.Domain.Interfaces
{
    public interface IHabitacionRepository
    {
        Task<Habitacion?> GetByIdAsync(Guid id);
        Task<Habitacion?> GetByNumberAsync(string number);
        Task<IEnumerable<Habitacion>> GetAllAsync();
        Task<Habitacion> AddAsync(Habitacion habitacion);
        Task<Habitacion> UpdateAsync(Habitacion habitacion);
        Task DeleteAsync(Guid id);
    }
}