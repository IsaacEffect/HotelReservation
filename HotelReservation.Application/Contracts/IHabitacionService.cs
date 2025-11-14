using HotelReservation.Application.Base.Result;
using HotelReservation.Application.Dtos;

namespace HotelReservation.Application.Contracts
{
    public interface IHabitacionService
    {
        Task<OperationResult<IEnumerable<ObtenerHabitacionDto>>> GetAllAsync();
        Task<OperationResult<ObtenerHabitacionDto>> GetByIdAsync(Guid id);
        Task<OperationResult<ObtenerHabitacionDto>> GetByNumberAsync(string number);
        Task<OperationResult<Guid>> AddAsync(InsertarHabitacionDto dto);
        Task<OperationResult> UpdateAsync(Guid id, ActualizarHabitacionDto dto);
        Task<OperationResult> UpdateStatusAsync(Guid id, string nuevoEstado);
        Task<OperationResult> DeleteAsync(Guid id);
    }
}