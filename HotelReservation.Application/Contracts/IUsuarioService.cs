using HotelReservation.Application.Dtos;

namespace HotelReservation.Application.Contracts
{
    public interface IUsuarioService
    {
        Task<IEnumerable<ObtenerUsuarioDto>> GetAllAsync();
        Task<ObtenerUsuarioDto?> GetByIdAsync(Guid id);
        Task<ObtenerUsuarioDto?> GetByEmailAsync(string correo);
        Task AddAsync(InsertarUsuarioDto usuario);
        Task UpdateAsync(ActualizarUsuarioDto usuario);
        Task DeleteAsync(Guid id);

        // validar credenciales (login)
        Task<(bool Success, string Message, ObtenerUsuarioDto? Data)> ValidarCredencialesAsync(string correo, string contrasena);
    }
}
