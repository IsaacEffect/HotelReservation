using HotelReservation.Application.Base.Result;
using HotelReservation.Application.Dtos;

namespace HotelReservation.Application.Contracts
{
    public interface IClienteService
    {
        Task<OperationResult<IEnumerable<ObtenerClienteDto>>> GetAllAsync();
        Task<OperationResult<ObtenerClienteDto>> GetByIdAsync(Guid id);
        Task<OperationResult> AddAsync(InsertarClienteDto cliente);
        Task<OperationResult> UpdateAsync(Guid id, ActualizarClienteDto cliente);
        Task<OperationResult> DeleteAsync(Guid id);
    }
}
