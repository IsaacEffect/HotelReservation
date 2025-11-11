using HotelReservation.Domain.Entities;

namespace HotelReservation.Domain.Interfaces
{
    public interface IHabitacionRepository
    {
        Task<Habitacion?> GetByIdAsync(Guid id);
        Task<Habitacion?> GetByNumberAsync(int number);
        Task<IEnumerable<Habitacion>> GetAllAsync();
        Task<Habitacion> AddAsync(Habitacion Habitacion);
        Task<Habitacion> UpdateAsync(Habitacion Habitacion);
        Task DeleteAsync(Guid id);
    }
}