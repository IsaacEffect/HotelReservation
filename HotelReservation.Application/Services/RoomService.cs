using HotelReservation.Application.Interfaces;
using HotelReservation.Application.DTOs;
using HotelReservation.Domain.Entities;

namespace HotelReservation.Application.Services
{
    public class RoomService
    {
        private readonly IRoomRepository _repo;
        public RoomService(IRoomRepository repo) => _repo = repo;

        public async Task<RoomDto> CreateRoomAsync(int number, Guid categoryId, decimal price)
        {
            var existing = await _repo.GetByNumberAsync(number);
            if (existing != null) throw new InvalidOperationException("Ya existe una habitación con ese número");

            var room = new Room(number, categoryId, price);
            await _repo.AddAsync(room);
            return new RoomDto(room.Id, room.Number, room.CategoryId, room.Status, room.Price);
        }

        public async Task<RoomDto?> GetRoomAsync(Guid id)
        {
            var r = await _repo.GetByIdAsync(id);
            if (r == null) return null;
            return new RoomDto(r.Id, r.Number, r.CategoryId, r.Status, r.Price);
        }

        public async Task<IEnumerable<RoomDto>> GetAllAsync()
        {
            var list = await _repo.GetAllAsync();
            return list.Select(r => new RoomDto(r.Id, r.Number, r.CategoryId, r.Status, r.Price));
        }

        public async Task UpdatePriceAsync(Guid id, decimal newPrice)
        {
            var r = await _repo.GetByIdAsync(id) ?? throw new KeyNotFoundException("Habitación no encontrada");
            r.UpdatePrice(newPrice);
            await _repo.UpdateAsync(r);
        }

        public async Task ChangeStatusAsync(Guid id, RoomStatus newStatus)
        {
            var r = await _repo.GetByIdAsync(id) ?? throw new KeyNotFoundException("Habitación no encontrada");
            r.ChangeStatus(newStatus);
            await _repo.UpdateAsync(r);
        }

        public async Task DeleteAsync(Guid id)
        {
            var r = await _repo.GetByIdAsync(id) ?? throw new KeyNotFoundException("Habitación no encontrada");
            await _repo.DeleteAsync(id);
        }
    }
}
