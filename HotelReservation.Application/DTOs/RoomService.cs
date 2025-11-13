using AutoMapper;
using HotelReservation.Application.DTOs;
using HotelReservation.Domain.Entities;
using HotelReservation.Persistence.Repositories; // ← ESTE ES EL CORRECTO
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelReservation.Application.DTOs
{
    public class RoomService : IRoomService
    {
        private readonly IHabitacionRepository _repo;
        private readonly IMapper _mapper;

        public RoomService(IHabitacionRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RoomDto>> GetAllAsync()
            => _mapper.Map<IEnumerable<RoomDto>>(await _repo.GetAllAsync());

        public async Task<RoomDto?> GetByIdAsync(int id)
            => _mapper.Map<RoomDto>(await _repo.GetByIdAsync(id));

        public async Task<RoomDto> AddAsync(RoomDto dto)
        {
            var room = _mapper.Map<Room>(dto);
            await _repo.AddAsync(room);
            await _repo.SaveChangesAsync();
            dto.Id = room.Id;
            return dto;
        }

        public async Task UpdateAsync(RoomDto dto)
        {
            var room = await _repo.GetByIdAsync(dto.Id);
            if (room == null) throw new KeyNotFoundException("Habitación no encontrada");
            _mapper.Map(dto, room);
            await _repo.UpdateAsync(room);
            await _repo.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await _repo.DeleteAsync(id);
            await _repo.SaveChangesAsync();
        }
    }
}