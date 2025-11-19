using HotelReservation.Application.Base.Result;
using HotelReservation.Application.Dtos;

namespace HotelReservation.Application.Contracts
{
    public interface IUsuarioService
    {
        Task<OperationResult<IEnumerable<ObtenerUsuarioDto>>> GetAllAsync();
        Task<OperationResult<ObtenerUsuarioDto>> GetByIdAsync(Guid id);
        Task<ObtenerUsuarioDto> GetByEmailAsync(string correo);
        Task<OperationResult> AddAsync(InsertarUsuarioDto usuario);
        Task<OperationResult> UpdateAsync(Guid id, ActualizarUsuarioDto usuario);
        Task<OperationResult> CambiarContrasenaAsync(CambiarContrasenaDto usuario);
        Task<OperationResult> DeleteAsync(Guid id);

        Task<(bool Success, string Message, ObtenerUsuarioDto? Data)> ValidarCredencialesAsync(string correo, string contrasena);
    }
}
