using HotelReservation.Application.Dtos;

namespace HotelReservation.Application.Contracts
{
    public interface IRolService
    {
        Task<IEnumerable<ObtenerRolDto>> GetAllAsync();
        Task<ObtenerRolDto?> GetByIdAsync(Guid id);
        Task AddAsync(InsertarRolDto rol);
    }
}
