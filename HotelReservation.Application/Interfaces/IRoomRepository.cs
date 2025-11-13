using HotelReservation.Domain.Entities;

namespace HotelReservation.Application.Interfaces
{
    public interface IRoomRepository
    {
        Task<Room?> GetByIdAsync(Guid id);
        Task<Room?> GetByNumberAsync(int number);
        Task<IEnumerable<Room>> GetAllAsync();
        Task AddAsync(Room room);
        Task UpdateAsync(Room room);
        Task DeleteAsync(Guid id);
    }
}
