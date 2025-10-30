using HotelReservation.Domain.Interfaces;
using HotelReservation.Domain.Entities;
using HotelReservation.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Persistence.Repositories
{
    public class CheckInOutRepository : ICheckInOutRepository
    {
        private readonly HotelReservationDbContext _context;
        public CheckInOutRepository(HotelReservationDbContext context)
        {
            _context = context;
        }

        public async Task<CheckInOut> AddAsync(CheckInOut entity)
        {
            _context.CheckInOuts.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(Guid id)
        {
            var ent = await _context.CheckInOuts.FindAsync(id);
            if (ent == null) return;
            _context.CheckInOuts.Remove(ent);
            await _context.SaveChangesAsync();
        }

        public async Task<CheckInOut?> GetByReservaIdAsync(Guid reservaId)
        {
            return await _context.CheckInOuts.FirstOrDefaultAsync(c => c.ReservaId == reservaId);
        }

        public async Task<CheckInOut> UpdateAsync(CheckInOut entity)
        {
            _context.CheckInOuts.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
