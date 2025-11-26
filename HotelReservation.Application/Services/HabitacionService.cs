using AutoMapper;
using HotelReservation.Application.Base.Result;
using HotelReservation.Application.Contracts;
using HotelReservation.Application.Dtos;
using HotelReservation.Domain.Entities;
using HotelReservation.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace HotelReservation.Application.Services
{
    public class HabitacionService : IHabitacionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<HabitacionService> _logger;

        public HabitacionService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<HabitacionService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<OperationResult<IEnumerable<ObtenerHabitacionDto>>> GetAllAsync()
        {
            try
            {
                var habitaciones = await _unitOfWork.Habitaciones.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<ObtenerHabitacionDto>>(habitaciones);

                return OperationResult<IEnumerable<ObtenerHabitacionDto>>.Ok(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en GetAll Habitaciones");
                return OperationResult<IEnumerable<ObtenerHabitacionDto>>.Fail("Error al obtener habitaciones.");
            }
        }

        public async Task<OperationResult<ObtenerHabitacionDto>> GetByIdAsync(Guid id)
        {
            try
            {
                var habitacion = await _unitOfWork.Habitaciones.GetByIdAsync(id);

                if (habitacion == null)
                    return OperationResult<ObtenerHabitacionDto>.Fail("Habitación no encontrada.");

                var dto = _mapper.Map<ObtenerHabitacionDto>(habitacion);

                return OperationResult<ObtenerHabitacionDto>.Ok(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en GetById Habitaciones");
                return OperationResult<ObtenerHabitacionDto>.Fail("Error al obtener habitación.");
            }
        }

        public async Task<OperationResult<ObtenerHabitacionDto>> GetByNumberAsync(string number)
        {
            try
            {
                var habitacion = await _unitOfWork.Habitaciones.GetByNumberAsync(number);

                if (habitacion == null)
                    return OperationResult<ObtenerHabitacionDto>.Fail("Habitación no encontrada.");

                var dto = _mapper.Map<ObtenerHabitacionDto>(habitacion);

                return OperationResult<ObtenerHabitacionDto>.Ok(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en GetByNumber Habitaciones");
                return OperationResult<ObtenerHabitacionDto>.Fail("Error al obtener habitación.");
            }
        }

        public async Task<OperationResult<Guid>> AddAsync(InsertarHabitacionDto dto)
        {
            try
            {
                var nueva = _mapper.Map<Habitacion>(dto);

                await _unitOfWork.Habitaciones.AddAsync(nueva);
                await _unitOfWork.SaveChangesAsync();

                return OperationResult<Guid>.Ok(nueva.Id, "Habitación creada exitosamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar habitación.");
                return OperationResult<Guid>.Fail("Error al agregar habitación.");
            }
        }

        public async Task<OperationResult> UpdateAsync(Guid id, ActualizarHabitacionDto dto)
        {
            try
            {
                var habitacion = await _unitOfWork.Habitaciones.GetByIdAsync(id);
                if (habitacion == null)
                    return OperationResult.Fail("Habitación no encontrada.");

                _mapper.Map(dto, habitacion);

                await _unitOfWork.Habitaciones.UpdateAsync(habitacion);
                await _unitOfWork.SaveChangesAsync();

                return OperationResult.Ok("Habitación actualizada.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar habitación.");
                return OperationResult.Fail("Error al actualizar habitación.");
            }
        }

        public async Task<OperationResult> UpdateStatusAsync(Guid id, string nuevoEstado)
        {
            try
            {
                var habitacion = await _unitOfWork.Habitaciones.GetByIdAsync(id);
                if (habitacion == null)
                    return OperationResult.Fail("Habitación no encontrada.");

                habitacion.Estado = nuevoEstado;

                await _unitOfWork.Habitaciones.UpdateAsync(habitacion);
                await _unitOfWork.SaveChangesAsync();

                return OperationResult.Ok("Estado actualizado.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar estado.");
                return OperationResult.Fail("Error al actualizar estado.");
            }
        }

        public async Task<OperationResult> DeleteAsync(Guid id)
        {
            try
            {
                var habitacion = await _unitOfWork.Habitaciones.GetByIdAsync(id);
                if (habitacion == null)
                    return OperationResult.Fail("Habitación no encontrada.");

                await _unitOfWork.Habitaciones.DeleteAsync(id);
                await _unitOfWork.SaveChangesAsync();

                return OperationResult.Ok("Habitación eliminada.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar habitación.");
                return OperationResult.Fail("Error al eliminar habitación.");
            }
        }
    }
}
