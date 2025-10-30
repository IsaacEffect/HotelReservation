using Microsoft.EntityFrameworkCore;
using HotelReservation.Domain.Entities;
using HotelReservation.Domain.Interfaces;
using HotelReservation.Persistence.Context;

namespace HotelReservation.Persistence.Repositories
{
    public class ReservaRepository : IReservaRepository
    {
        private readonly HotelReservationDbContext _context;

        public ReservaRepository(HotelReservationDbContext context)
        {
            _context = context;
        }

        public async Task<Reserva?> GetByIdAsync(Guid id)
        {
            return await _context.Reservas!
                .Include(r => r.Cliente)
                .Include(r => r.Habitacion)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Reserva>> GetAllAsync()
        {
            return await _context.Reservas!.ToListAsync();
        }

        public async Task<Reserva> AddAsync(Reserva entity)
        {
            _context.Reservas!.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Reserva> UpdateAsync(Reserva entity)
        {
            _context.Reservas!.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _context.Reservas!.FindAsync(id);
            if (entity != null)
            {
                _context.Reservas!.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
