using HotelReservation.Domain.Entities;

namespace HotelReservation.Domain.Interfaces
{
    public interface ICategoriaHabitacionRepository
    {
        Task<IEnumerable<CategoriaHabitacion>> GetAllAsync();
        Task<CategoriaHabitacion?> GetByIdAsync(Guid id);
        Task AddAsync(CategoriaHabitacion categoria);
        Task UpdateAsync(CategoriaHabitacion categoria);
        Task DeleteAsync(Guid id);
    }
}