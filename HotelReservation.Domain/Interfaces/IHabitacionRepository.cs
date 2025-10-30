using HotelReservation.Domain.Entities;

namespace HotelReservation.Domain.Interfaces
{
    public interface IHabitacionRepository
    {
        Task<Habitacion?> GetByIdAsync(Guid id);
        Task<IEnumerable<Habitacion>> GetAllAsync();
        Task<Habitacion> AddAsync(Habitacion entity);
        Task<Habitacion> UpdateAsync(Habitacion entity);
        Task DeleteAsync(Guid id);
    }

}
