using HotelReservation.Application.Dtos;

namespace HotelReservation.Application.Contracts
{
    public interface IClienteService
    {
        Task<IEnumerable<ObtenerClienteDto>> GetAllAsync();
        Task<ObtenerClienteDto> GetByIdAsync(Guid id);
        Task AddAsync(InsertarClienteDto cliente);
        Task UpdateAsync(Guid id, ActualizarClienteDto cliente);
        Task DeleteAsync(Guid id);
    }
}
