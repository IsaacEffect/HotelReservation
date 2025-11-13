using HotelReservation.Application.Base.Result;
using HotelReservation.Application.Dtos;

namespace HotelReservation.Application.Contracts
{
    public interface IRolService
    {
        Task<OperationResult<IEnumerable<ObtenerRolDto>>> GetAllAsync();
        Task<OperationResult<ObtenerRolDto>> GetByIdAsync(Guid id);
        Task<OperationResult> AddAsync(InsertarRolDto rol);
    }
}
