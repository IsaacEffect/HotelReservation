using AutoMapper;
using HotelReservation.Application.Base.Result;
using HotelReservation.Application.Contracts;
using HotelReservation.Application.Dtos;
using HotelReservation.Domain.Entities;
using HotelReservation.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace HotelReservation.Application.Services
{
    public class CategoriaHabitacionService : ICategoriaHabitacionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoriaHabitacionService> _logger;

        public CategoriaHabitacionService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CategoriaHabitacionService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<OperationResult<IEnumerable<ObtenerCategoriaDto>>> GetAllAsync()
        {
            try
            {
                var categorias = await _unitOfWork.Categorias.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<ObtenerCategoriaDto>>(categorias);
                return OperationResult<IEnumerable<ObtenerCategoriaDto>>.Ok(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener categorías.");
                return OperationResult<IEnumerable<ObtenerCategoriaDto>>.Fail("Error al obtener categorías.");
            }
        }

        public async Task<OperationResult<ObtenerCategoriaDto>> GetByIdAsync(Guid id)
        {
            try
            {
                var categoria = await _unitOfWork.Categorias.GetByIdAsync(id);
                if (categoria == null)
                {
                    _logger.LogWarning("Categoría con ID {Id} no encontrada.", id);
                    return OperationResult<ObtenerCategoriaDto>.Fail("Categoría no encontrada.");
                }

                var dto = _mapper.Map<ObtenerCategoriaDto>(categoria);
                return OperationResult<ObtenerCategoriaDto>.Ok(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener categoría por ID {Id}", id);
                return OperationResult<ObtenerCategoriaDto>.Fail("Error al obtener categoría.");
            }
        }

        public async Task<OperationResult<Guid>> AddAsync(InsertarCategoriaDto dto)
        {
            try
            {
                var categoria = _mapper.Map<CategoriaHabitacion>(dto);

                // Validar duplicado
                var existe = await _unitOfWork.Categorias.GetByNameAsync(categoria.NombreCategoria);
                if (existe != null)
                    return OperationResult<Guid>.Fail("El nombre de la categoría ya existe.");

                await _unitOfWork.Categorias.AddAsync(categoria);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Categoría {Name} creada con ID {Id}", categoria.NombreCategoria, categoria.Id);
                return OperationResult<Guid>.Ok(categoria.Id, "Categoría creada exitosamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar categoría.");
                return OperationResult<Guid>.Fail("Error al agregar la categoría.");
            }
        }

        public async Task<OperationResult> UpdateAsync(Guid id, ActualizarCategoriaDto dto)
        {
            try
            {
                var categoria = await _unitOfWork.Categorias.GetByIdAsync(id);
                if (categoria == null)
                {
                    _logger.LogWarning("Update: Categoría con ID {Id} no encontrada.", id);
                    return OperationResult.Fail("Categoría no encontrada.");
                }

                _mapper.Map(dto, categoria); // Actualiza la entidad existente con datos del DTO

                // Validar duplicado en update
                var existe = await _unitOfWork.Categorias.GetByNameAsync(categoria.NombreCategoria);
                if (existe != null && existe.Id != id)
                    return OperationResult.Fail("El nombre de la categoría ya existe.");

                await _unitOfWork.Categorias.UpdateAsync(categoria);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Categoría {Id} actualizada.", id);
                return OperationResult.Ok("Categoría actualizada exitosamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar categoría {Id}", id);
                return OperationResult.Fail("Error al actualizar la categoría.");
            }
        }

        public async Task<OperationResult> DeleteAsync(Guid id)
        {
            try
            {
                var categoria = await _unitOfWork.Categorias.GetByIdAsync(id);
                if (categoria == null)
                {
                    _logger.LogWarning("Delete: Categoría con ID {Id} no encontrada.", id);
                    return OperationResult.Fail("Categoría no encontrada.");
                }

                await _unitOfWork.Categorias.DeleteAsync(id);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Categoría {Id} eliminada.", id);
                return OperationResult.Ok("Categoría eliminada exitosamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar categoría {Id}", id);
                return OperationResult.Fail("Error al eliminar la categoría.");
            }
        }
    }
}