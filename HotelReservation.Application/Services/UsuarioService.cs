using AutoMapper;
using HotelReservation.Application.Contracts;
using HotelReservation.Application.Dtos;
using HotelReservation.Domain.Entities;
using HotelReservation.Domain.Interfaces;

namespace HotelReservation.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UsuarioService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ObtenerUsuarioDto>> GetAllAsync()
        {
            var usuarios = await _unitOfWork.Usuarios.GetAllAsync();
            return _mapper.Map<IEnumerable<ObtenerUsuarioDto>>(usuarios);
        }

        public async Task<ObtenerUsuarioDto?> GetByIdAsync(Guid id)
        {
            var usuario = await _unitOfWork.Usuarios.GetByIdAsync(id);
            return _mapper.Map<ObtenerUsuarioDto>(usuario);
        }

        public async Task<ObtenerUsuarioDto?> GetByEmailAsync(string correo)
        {
            var usuario = await _unitOfWork.Usuarios.GetByEmailAsync(correo);
            return _mapper.Map<ObtenerUsuarioDto>(usuario);
        }

        public async Task AddAsync(InsertarUsuarioDto usuarioDto)
        {
            var usuario = _mapper.Map<Usuario>(usuarioDto);
            await _unitOfWork.Usuarios.AddAsync(usuario);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(ActualizarUsuarioDto usuarioDto)
        {
            var usuario = _mapper.Map<Usuario>(usuarioDto);
            await _unitOfWork.Usuarios.UpdateAsync(usuario);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            await _unitOfWork.Usuarios.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<(bool Success, string Message, ObtenerUsuarioDto? Data)> ValidarCredencialesAsync(string correo, string contrasena)
        {
            var usuario = await _unitOfWork.Usuarios.GetByEmailAsync(correo);
            if (usuario == null)
            {
                return (false, "Usuario no encontrado.", null);
            }
            if (usuario.Contrasena != contrasena)
            {
                return (false, "Contraseña incorrecta.", null);
            }
            if (!usuario.Estado)
            {
                return (false, "El usuario está inactivo.", null);
            }
            var usuarioDto = _mapper.Map<ObtenerUsuarioDto>(usuario);
            return (true, "Inicio de sesión exitoso.", usuarioDto);
        }
    }
}