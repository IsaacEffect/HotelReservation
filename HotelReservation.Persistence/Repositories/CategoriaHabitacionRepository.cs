using HotelReservation.Domain.Entities;
using HotelReservation.Domain.Interfaces;
using HotelReservation.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Persistence.Repositories
{
    public class CategoriaHabitacionRepository : ICategoriaHabitacionRepository
    {
        private readonly HotelReservationDBContext _context;

        public CategoriaHabitacionRepository(HotelReservationDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CategoriaHabitacion>> GetAllAsync()
        {
            return await _context.CategoriasHabitacion.ToListAsync();
        }

        public async Task<CategoriaHabitacion?> GetByIdAsync(Guid id)
        {
            return await _context.CategoriasHabitacion.FindAsync(id);
        }

        public async Task<CategoriaHabitacion?> GetByNameAsync(string nombre)
        {
            return await _context.CategoriasHabitacion
                .FirstOrDefaultAsync(c => c.NombreCategoria == nombre);
        }

        public async Task AddAsync(CategoriaHabitacion categoria)
        {
            await _context.CategoriasHabitacion.AddAsync(categoria);
        }

        public async Task UpdateAsync(CategoriaHabitacion categoria)
        {
            _context.CategoriasHabitacion.Update(categoria);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Guid id)
        {
            var categoria = await _context.CategoriasHabitacion.FindAsync(id);
            if (categoria != null)
            {
                _context.CategoriasHabitacion.Remove(categoria);
            }
        }
    }
}