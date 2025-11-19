using Microsoft.EntityFrameworkCore;
using HotelReservation.Domain.Entities;
using HotelReservation.Domain.Interfaces;
using HotelReservation.Persistence.Context;

namespace HotelReservation.Persistence.Repositories
{
    public class HabitacionRepository : IHabitacionRepository
    {
        private readonly HotelReservationDBContext _context;

        public HabitacionRepository(HotelReservationDBContext context)
        {
            _context = context;
        }

        public async Task<Habitacion?> GetByIdAsync(Guid id)
        {
            return await _context.Habitaciones!.FirstOrDefaultAsync(h => h.Id == id);
        }

<<<<<<< HEAD
        public async Task<Habitacion?> GetByNumberAsync(int number)
        {
            return await _context.Habitaciones!
                .AsNoTracking()
                .FirstOrDefaultAsync(h => h.Number == number);
=======
        public async Task<Habitacion?> GetByNumberAsync(string number)
        {
            return await _context.Habitaciones!
                .AsNoTracking()
                .FirstOrDefaultAsync(h => h.Numero == number);
>>>>>>> origin/develop
        }

        public async Task<IEnumerable<Habitacion>> GetAllAsync()
        {
            return await _context.Habitaciones!.ToListAsync();
        }

        public async Task<Habitacion> AddAsync(Habitacion entity)
        {
            _context.Habitaciones!.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Habitacion> UpdateAsync(Habitacion entity)
        {
            _context.Habitaciones!.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _context.Habitaciones!.FindAsync(id);
            if (entity != null)
            {
                _context.Habitaciones!.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}