using HotelReservation.Domain.Entities;

namespace HotelReservation.Application.Contracts
{
    public interface IUsuarioService
    {
        Task<IEnumerable<Usuario>> GetAllAsync();
        Task<Usuario?> GetByIdAsync(Guid id);
        Task<Usuario?> GetByCorreoAsync(string correo);
        Task AddAsync(Usuario usuario);
        Task UpdateAsync(Usuario usuario);
        Task DeleteAsync(Guid id);
    }
}
