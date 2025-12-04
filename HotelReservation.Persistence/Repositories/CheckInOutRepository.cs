using HotelReservation.Domain.Interfaces;
using HotelReservation.Domain.Entities;
using HotelReservation.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Persistence.Repositories
{
    public class CheckInOutRepository : ICheckInOutRepository
    {
        private readonly HotelReservationDBContext _context;
        public CheckInOutRepository(HotelReservationDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CheckInOut>> GetAllAsync()
        {
            return await _context.CheckInOut.ToListAsync();
        }

        public async Task<CheckInOut?> GetByIdAsync(Guid id)
        {
            return await _context.CheckInOut.FindAsync(id);
        }

        public async Task<CheckInOut> AddAsync(CheckInOut entity)
        {
            _context.CheckInOut.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(Guid id)
        {
            var ent = await _context.CheckInOut.FindAsync(id);
            if (ent == null) return;
            _context.CheckInOut.Remove(ent);
            await _context.SaveChangesAsync();
        }

        public async Task<CheckInOut?> GetByReservaIdAsync(Guid reservaId)
        {
            return await _context.CheckInOut.FirstOrDefaultAsync(c => c.ReservaId == reservaId);
        }

        public async Task<CheckInOut> UpdateAsync(CheckInOut entity)
        {
            _context.CheckInOut.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}