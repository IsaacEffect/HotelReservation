using HotelReservation.Application.Contracts;
using HotelReservation.Domain.Entities;
using HotelReservation.Domain.Interfaces;

namespace HotelReservation.Application.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClienteService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Cliente>> GetAllAsync()
        {
            return await _unitOfWork.Clientes.GetAllAsync();
        }

        public async Task<Cliente> GetByIdAsync(Guid id)
        {
            return await _unitOfWork.Clientes.GetByIdAsync(id);
        }

        public async Task AddAsync(Cliente cliente)
        {
            await _unitOfWork.Clientes.AddAsync(cliente);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(Cliente cliente)
        {
            await _unitOfWork.Clientes.UpdateAsync(cliente);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            await _unitOfWork.Clientes.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
