using AutoMapper;
using HotelReservation.Application.Contracts;
using HotelReservation.Application.Dtos;
using HotelReservation.Domain.Entities;
using HotelReservation.Domain.Interfaces;
using System.Security.Cryptography;
using System.Text;

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
            usuario.Contrasena = HashPassword(usuario.Contrasena);
            await _unitOfWork.Usuarios.AddAsync(usuario);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(Guid id, ActualizarUsuarioDto usuarioDto)
        {
            var usuarioExistente = await _unitOfWork.Usuarios.GetByIdAsync(id);
            if (usuarioExistente == null) throw new Exception("Usuario no encontrado.");

            usuarioExistente.Nombre = usuarioDto.Nombre;
            usuarioExistente.Apellido = usuarioDto.Apellido;
            usuarioExistente.Correo = usuarioDto.Correo;
            usuarioExistente.Estado = usuarioDto.Estado;
            usuarioExistente.RolId = usuarioDto.RolId;

            await _unitOfWork.Usuarios.UpdateAsync(usuarioExistente);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<(bool Success, string Message)> CambiarContrasenaAsync(CambiarContrasenaDto dto)
        {
            var usuario = await _unitOfWork.Usuarios.GetByIdAsync(dto.IdUsuario);
            if (usuario == null)
                return (false, "Usuario no encontrado.");

            var contrasenaActualHash = HashPassword(dto.ContrasenaActual);
            if (usuario.Contrasena != contrasenaActualHash)
                return (false, "La contraseña actual es incorrecta.");

            usuario.Contrasena = HashPassword(dto.NuevaContrasena);
            await _unitOfWork.Usuarios.UpdateAsync(usuario);
            await _unitOfWork.SaveChangesAsync();

            return (true, "Contraseña cambiada correctamente.");
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
                return (false, "Usuario no encontrado.", null);

            var contrasenaHash = HashPassword(contrasena);
            if (usuario.Contrasena != contrasenaHash)
                return (false, "Contraseña incorrecta.", null);

            if (!usuario.Estado)
                return (false, "El usuario está inactivo.", null);

            var usuarioDto = _mapper.Map<ObtenerUsuarioDto>(usuario);
            return (true, "Inicio de sesión exitoso.", usuarioDto);
        }

        // Encriptar contraseña con SHA256
        private static string HashPassword(string password)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
}
