using AutoMapper;
using HotelReservation.Application.Base.Result;
using HotelReservation.Application.Contracts;
using HotelReservation.Application.Dtos;
using HotelReservation.Domain.Entities;
using HotelReservation.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace HotelReservation.Application.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ClienteService> _logger;

        public ClienteService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ClienteService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<OperationResult<IEnumerable<ObtenerClienteDto>>> GetAllAsync()
        {
            try
            {
                var clientes = await _unitOfWork.Clientes.GetAllAsync();
                var dto = _mapper.Map<IEnumerable<ObtenerClienteDto>>(clientes);
                _logger.LogInformation("Se obtuvieron {Count} clientes activos.", dto.Count());
                return OperationResult<IEnumerable<ObtenerClienteDto>>.Ok(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los clientes.");
                return OperationResult<IEnumerable<ObtenerClienteDto>>.Fail("Error al obtener los clientes.");
            }
        }

        public async Task<OperationResult<ObtenerClienteDto>> GetByIdAsync(Guid id)
        {
            try
            {
                var cliente = await _unitOfWork.Clientes.GetByIdAsync(id);
                var dto = _mapper.Map<ObtenerClienteDto>(cliente);
                return OperationResult<ObtenerClienteDto>.Ok(dto);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Cliente con ID {Id} no encontrado.", id);
                return OperationResult<ObtenerClienteDto>.Fail(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener cliente por ID {Id}.", id);
                return OperationResult<ObtenerClienteDto>.Fail("Error interno del servidor.");
            }
        }

        public async Task<OperationResult> AddAsync(InsertarClienteDto clienteDto)
        {
            try
            {
                var cliente = _mapper.Map<Cliente>(clienteDto);
                await _unitOfWork.Clientes.AddAsync(cliente);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Cliente {Nombre} agregado exitosamente.", cliente.Nombre);
                return OperationResult.Ok("Cliente agregado correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar cliente.");
                return OperationResult.Fail("Error al agregar el cliente.");
            }
        }

        public async Task<OperationResult> UpdateAsync(Guid id, ActualizarClienteDto clienteDto)
        {
            try
            {
                var clienteExistente = await _unitOfWork.Clientes.GetByIdAsync(id);
                _mapper.Map(clienteDto, clienteExistente);
                await _unitOfWork.Clientes.UpdateAsync(clienteExistente);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Cliente {Id} actualizado correctamente.", id);
                return OperationResult.Ok("Cliente modificado correctamente.");
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "No se encontró el cliente {Id}.", id);
                return OperationResult.Fail(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar cliente {Id}.", id);
                return OperationResult.Fail("Error al modificar el cliente.");
            }
        }

        public async Task<OperationResult> DeleteAsync(Guid id)
        {
            try
            {
                await _unitOfWork.Clientes.DeleteAsync(id);
                await _unitOfWork.SaveChangesAsync();
                _logger.LogInformation("Cliente {Id} eliminado (estado = false).", id);
                return OperationResult.Ok("Cliente eliminado correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar cliente {Id}.", id);
                return OperationResult.Fail("Error al eliminar el cliente.");
            }
        }
    }
}
