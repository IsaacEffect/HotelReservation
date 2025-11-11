using HotelReservation.Application.Contracts;
using HotelReservation.Domain.Entities;
using HotelReservation.Domain.Interfaces;

namespace HotelReservation.Application.Services
{
    public class RolService : IRolService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RolService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Rol>> GetAllAsync()
            => await _unitOfWork.Roles.GetAllAsync();

        public async Task<Rol?> GetByIdAsync(Guid id)
            => await _unitOfWork.Roles.GetByIdAsync(id);

        public async Task AddAsync(Rol rol)
        {
            await _unitOfWork.Roles.AddAsync(rol);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
