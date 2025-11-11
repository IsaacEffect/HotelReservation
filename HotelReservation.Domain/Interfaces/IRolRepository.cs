using HotelReservation.Domain.Entities;

namespace HotelReservation.Domain.Interfaces
{
    public interface IRolRepository
    {
        Task<IEnumerable<Rol>> GetAllAsync();
        Task<Rol?> GetByIdAsync(Guid id);
        Task AddAsync(Rol rol);
    }
}
