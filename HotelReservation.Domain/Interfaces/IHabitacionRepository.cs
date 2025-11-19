using HotelReservation.Domain.Entities;

namespace HotelReservation.Domain.Interfaces
{
    public interface IHabitacionRepository
    {
        Task<Habitacion?> GetByIdAsync(Guid id);
<<<<<<< HEAD
        Task<Habitacion?> GetByNumberAsync(int number);
        Task<IEnumerable<Habitacion>> GetAllAsync();
        Task<Habitacion> AddAsync(Habitacion Habitacion);
        Task<Habitacion> UpdateAsync(Habitacion Habitacion);
=======
        Task<Habitacion?> GetByNumberAsync(string number);
        Task<IEnumerable<Habitacion>> GetAllAsync();
        Task<Habitacion> AddAsync(Habitacion habitacion);
        Task<Habitacion> UpdateAsync(Habitacion habitacion);
>>>>>>> origin/develop
        Task DeleteAsync(Guid id);
    }
}