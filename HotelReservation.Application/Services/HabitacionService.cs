using HotelReservation.Application.DTOs;
using HotelReservation.Domain.Entities;
using HotelReservation.Domain.Interfaces;

namespace HotelReservation.Application.Services
{
    public class HabitacionService
    {
        private readonly IHabitacionRepository _repo;
        public HabitacionService(IHabitacionRepository repo) => _repo = repo;

        public async Task<HabitacionDto> CreateHabitacionAsync(int number, Guid categoryId, decimal price)
        {
            var existing = await _repo.GetByNumberAsync(number);
            if (existing != null) throw new InvalidOperationException("Ya existe una habitación con ese número");

            var Habitacion = new Habitacion(number, categoryId, price);
            await _repo.AddAsync(Habitacion);
            return new HabitacionDto(Habitacion.Id, Habitacion.Number, Habitacion.CategoryId, Habitacion.Status, Habitacion.Price);
        }

        public async Task<HabitacionDto?> GetHabitacionAsync(Guid id)
        {
            var r = await _repo.GetByIdAsync(id);
            if (r == null) return null;
            return new HabitacionDto(r.Id, r.Number, r.CategoryId, r.Status, r.Price);
        }

        public async Task<IEnumerable<HabitacionDto>> GetAllAsync()
        {
            var list = await _repo.GetAllAsync();
            return list.Select(r => new HabitacionDto(r.Id, r.Number, r.CategoryId, r.Status, r.Price));
        }

        public async Task UpdatePriceAsync(Guid id, decimal newPrice)
        {
            var r = await _repo.GetByIdAsync(id) ?? throw new KeyNotFoundException("Habitación no encontrada");
            r.UpdatePrice(newPrice);
            await _repo.UpdateAsync(r);
        }

        public async Task ChangeStatusAsync(Guid id, HabitacionStatus newStatus)
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