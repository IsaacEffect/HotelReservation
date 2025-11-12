using AutoMapper;
using HotelReservation.Application.Contracts;
using HotelReservation.Application.Dtos;
using HotelReservation.Domain.Entities;
using HotelReservation.Domain.Interfaces;

namespace HotelReservation.Application.Services
{
    public class ClienteService(IUnitOfWork unitOfWork, IMapper mapper) : IClienteService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<ObtenerClienteDto>> GetAllAsync()
        {
            var clientes = await _unitOfWork.Clientes.GetAllAsync();
            return _mapper.Map<IEnumerable<ObtenerClienteDto>>(clientes);
        }

        public async Task<ObtenerClienteDto> GetByIdAsync(Guid id)
        {
            var cliente = await _unitOfWork.Clientes.GetByIdAsync(id);
            return cliente == null
                ? throw new KeyNotFoundException($"Cliente con id {id} no encontrado")
                : _mapper.Map<ObtenerClienteDto>(cliente);
        }

        public async Task AddAsync(InsertarClienteDto clienteDto)
        {
            var cliente = _mapper.Map<Cliente>(clienteDto);
            await _unitOfWork.Clientes.AddAsync(cliente);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(Guid id, ActualizarClienteDto clienteDto)
        {
            var clienteExistente = await _unitOfWork.Clientes.GetByIdAsync(id);
            if (clienteExistente == null)
                throw new KeyNotFoundException($"Cliente con id {id} no encontrado.");

            _mapper.Map(clienteDto, clienteExistente);

            await _unitOfWork.Clientes.UpdateAsync(clienteExistente);
            await _unitOfWork.SaveChangesAsync();
        }


        public async Task DeleteAsync(Guid id)
        {
            await _unitOfWork.Clientes.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}