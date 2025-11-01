using HotelReservation.Application.Contracts;
using HotelReservation.Domain.Entities;
using HotelReservation.Domain.Interfaces;

namespace HotelReservation.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UsuarioService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Usuario>> GetAllAsync()
            => await _unitOfWork.Usuarios.GetAllAsync();

        public async Task<Usuario?> GetByIdAsync(Guid id)
            => await _unitOfWork.Usuarios.GetByIdAsync(id);

        public async Task<Usuario?> GetByCorreoAsync(string correo)
            => await _unitOfWork.Usuarios.GetByEmailAsync(correo);

        public async Task AddAsync(Usuario usuario)
        {
            await _unitOfWork.Usuarios.AddAsync(usuario);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(Usuario usuario)
        {
            await _unitOfWork.Usuarios.UpdateAsync(usuario);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            await _unitOfWork.Usuarios.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
