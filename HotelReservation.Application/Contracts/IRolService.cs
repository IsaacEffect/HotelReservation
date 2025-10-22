using HotelReservation.Domain.Entities;

namespace HotelReservation.Application.Contracts
{
    public interface IRolService
    {
        Task<IEnumerable<Rol>> GetAllAsync();
        Task<Rol?> GetByIdAsync(Guid id);
        Task AddAsync(Rol rol);
    }
}
