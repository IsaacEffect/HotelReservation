using AutoMapper;
using HotelReservation.Application.Contracts;
using HotelReservation.Application.Dtos;
using HotelReservation.Domain.Entities;
using HotelReservation.Domain.Interfaces;

namespace HotelReservation.Application.Services
{
    public class RolService : IRolService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RolService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ObtenerRolDto>> GetAllAsync()
        {
            var roles = await _unitOfWork.Roles.GetAllAsync();
            return _mapper.Map<IEnumerable<ObtenerRolDto>>(roles);
        }

        public async Task<ObtenerRolDto?> GetByIdAsync(Guid id)
        {
            var rol = await _unitOfWork.Roles.GetByIdAsync(id);
            return _mapper.Map<ObtenerRolDto>(rol);
        }

        public async Task AddAsync(InsertarRolDto rolDto)
        {
            var rol = _mapper.Map<Rol>(rolDto);
            await _unitOfWork.Roles.AddAsync(rol);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}