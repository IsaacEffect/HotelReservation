using AutoMapper;
using HotelReservation.Application.Base.Result;
using HotelReservation.Application.Contracts;
using HotelReservation.Application.Dtos;
using HotelReservation.Domain.Entities;
using HotelReservation.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace HotelReservation.Application.Services
{
    public class RolService : IRolService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<RolService> _logger;

        public RolService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<RolService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<OperationResult<IEnumerable<ObtenerRolDto>>> GetAllAsync()
        {
            try
            {
                var roles = await _unitOfWork.Roles.GetAllAsync();
                var dto = _mapper.Map<IEnumerable<ObtenerRolDto>>(roles);
                _logger.LogInformation("Se obtuvieron {Count} roles.", dto.Count());
                return OperationResult<IEnumerable<ObtenerRolDto>>.Ok(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los roles.");
                return OperationResult<IEnumerable<ObtenerRolDto>>.Fail("Error al obtener los roles.");
            }
        }

        public async Task<OperationResult<ObtenerRolDto>> GetByIdAsync(Guid id)
        {
            try
            {
                var rol = await _unitOfWork.Roles.GetByIdAsync(id);
                var dto = _mapper.Map<ObtenerRolDto>(rol);
                return OperationResult<ObtenerRolDto>.Ok(dto);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Rol con ID {Id} no encontrado.", id);
                return OperationResult<ObtenerRolDto>.Fail(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener rol por ID {Id}.", id);
                return OperationResult<ObtenerRolDto>.Fail("Error interno del servidor.");
            }
        }

        public async Task<OperationResult> AddAsync(InsertarRolDto rolDto)
        {
            try
            {
                var rol = _mapper.Map<Rol>(rolDto);
                await _unitOfWork.Roles.AddAsync(rol);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Rol {Nombre} agregado exitosamente.", rol.NombreRol);
                return OperationResult.Ok("Rol agregado correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar rol.");
                return OperationResult.Fail("Error al agregar el rol.");
            }
        }
    }
}