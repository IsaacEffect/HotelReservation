using AutoMapper;
using HotelReservation.Application.Base.Result;
using HotelReservation.Application.Contracts;
using HotelReservation.Application.Dtos;
using HotelReservation.Domain.Entities;
using HotelReservation.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;

namespace HotelReservation.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UsuarioService> _logger;

        public UsuarioService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UsuarioService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<OperationResult<IEnumerable<ObtenerUsuarioDto>>> GetAllAsync()
        {
            try
            {
                _logger.LogInformation("Obteniendo todos los usuarios...");
                var usuarios = await _unitOfWork.Usuarios.GetAllAsync();
                var data = _mapper.Map<IEnumerable<ObtenerUsuarioDto>>(usuarios);
                return OperationResult<IEnumerable<ObtenerUsuarioDto>>.Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener usuarios.");
                return OperationResult<IEnumerable<ObtenerUsuarioDto>>.Fail("Error al obtener usuarios.");
            }
        }

        public async Task<OperationResult<ObtenerUsuarioDto>> GetByIdAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("Buscando usuario por ID: {Id}", id);
                var usuario = await _unitOfWork.Usuarios.GetByIdAsync(id);
                if (usuario == null)
                {
                    _logger.LogWarning("Usuario con ID {Id} no encontrado.", id);
                    return OperationResult<ObtenerUsuarioDto>.Fail("Usuario no encontrado.");
                }

                return OperationResult<ObtenerUsuarioDto>.Ok(_mapper.Map<ObtenerUsuarioDto>(usuario));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener usuario por ID {Id}", id);
                return OperationResult<ObtenerUsuarioDto>.Fail("Error interno al obtener usuario.");
            }
        }

        public async Task<ObtenerUsuarioDto> GetByEmailAsync(string correo)
        {
            _logger.LogInformation("Buscando usuario por correo: {Correo}", correo);
            var usuario = await _unitOfWork.Usuarios.GetByEmailAsync(correo);
            return _mapper.Map<ObtenerUsuarioDto>(usuario);
        }

        public async Task<OperationResult> AddAsync(InsertarUsuarioDto usuarioDto)
        {
            try
            {
                _logger.LogInformation("Insertando nuevo usuario: {Correo}", usuarioDto.Correo);

                var usuario = _mapper.Map<Usuario>(usuarioDto);
                usuario.Contrasena = HashPassword(usuario.Contrasena);

                await _unitOfWork.Usuarios.AddAsync(usuario);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Usuario insertado correctamente: {Correo}", usuarioDto.Correo);
                return OperationResult.Ok("Usuario registrado correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al insertar usuario {Correo}", usuarioDto.Correo);
                return OperationResult.Fail("Error al registrar usuario.");
            }
        }

        public async Task<OperationResult> UpdateAsync(Guid id, ActualizarUsuarioDto usuarioDto)
        {
            try
            {
                _logger.LogInformation("Actualizando usuario con ID: {Id}", id);

                var usuarioExistente = await _unitOfWork.Usuarios.GetByIdAsync(id);
                if (usuarioExistente == null)
                    return OperationResult.Fail("Usuario no encontrado.");

                usuarioExistente.Nombre = usuarioDto.Nombre;
                usuarioExistente.Apellido = usuarioDto.Apellido;
                usuarioExistente.Correo = usuarioDto.Correo;
                usuarioExistente.RolId = usuarioDto.RolId;

                await _unitOfWork.Usuarios.UpdateAsync(usuarioExistente);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Usuario con ID {Id} actualizado correctamente.", id);
                return OperationResult.Ok("Usuario modificado correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar usuario con ID {Id}", id);
                return OperationResult.Fail("Error al actualizar usuario.");
            }
        }

        public async Task<OperationResult> CambiarContrasenaAsync(CambiarContrasenaDto dto)
        {
            try
            {
                _logger.LogInformation("Cambiando contraseña para usuario ID: {IdUsuario}", dto.IdUsuario);

                var usuario = await _unitOfWork.Usuarios.GetByIdAsync(dto.IdUsuario);
                if (usuario == null)
                    return OperationResult.Fail("Usuario no encontrado.");

                if (usuario.Contrasena != HashPassword(dto.ContrasenaActual))
                    return OperationResult.Fail("La contraseña actual es incorrecta.");

                usuario.Contrasena = HashPassword(dto.NuevaContrasena);
                await _unitOfWork.Usuarios.UpdateAsync(usuario);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Contraseña cambiada correctamente para usuario {IdUsuario}", dto.IdUsuario);
                return OperationResult.Ok("Contraseña cambiada correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cambiar contraseña para usuario {IdUsuario}", dto.IdUsuario);
                return OperationResult.Fail("Error al cambiar la contraseña.");
            }
        }

        public async Task<OperationResult> DeleteAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("Eliminando usuario con ID: {Id}", id);

                await _unitOfWork.Usuarios.DeleteAsync(id);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Usuario con ID {Id} eliminado correctamente.", id);
                return OperationResult.Ok("Usuario eliminado correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar usuario con ID {Id}", id);
                return OperationResult.Fail("Error al eliminar usuario.");
            }
        }

        public async Task<(bool Success, string Message, ObtenerUsuarioDto? Data)> ValidarCredencialesAsync(string correo, string contrasena)
        {
            try
            {
                _logger.LogInformation("Validando credenciales para usuario: {Correo}", correo);

                var usuario = await _unitOfWork.Usuarios.GetByEmailAsync(correo);
                if (usuario == null)
                    return (false, "Usuario no encontrado.", null);

                if (usuario.Contrasena != HashPassword(contrasena))
                    return (false, "Contraseña incorrecta.", null);

                if (!usuario.Estado)
                    return (false, "El usuario está inactivo.", null);

                var dto = _mapper.Map<ObtenerUsuarioDto>(usuario);
                _logger.LogInformation("Usuario {Correo} autenticado correctamente.", correo);
                return (true, "Inicio de sesión exitoso.", dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al validar credenciales de {Correo}", correo);
                return (false, "Error interno al validar credenciales.", null);
            }
        }

        private static string HashPassword(string password)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
}
