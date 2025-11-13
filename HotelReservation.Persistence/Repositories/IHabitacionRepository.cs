using HotelReservation.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelReservation.Persistence.Repositories
{
    public interface IHabitacionRepository
    {
        Task<IEnumerable<Room>> GetAllAsync();
        Task<Room?> GetByIdAsync(int id);
        Task AddAsync(Room room);
        Task UpdateAsync(Room room);
        Task DeleteAsync(int id);
        Task SaveChangesAsync();
    }
}