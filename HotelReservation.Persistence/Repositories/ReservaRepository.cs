using Microsoft.EntityFrameworkCore;
using HotelReservation.Domain.Entities;
using HotelReservation.Domain.Interfaces;
using HotelReservation.Persistence.Context;

namespace HotelReservation.Persistence.Repositories
{
    public class ReservaRepository : IReservaRepository
    {
        private readonly HotelReservationDBContext _context;

        public ReservaRepository(HotelReservationDBContext context)
        {
            _context = context;
        }

        public async Task<Reserva?> GetByIdAsync(Guid id)
        {
            return await _context.Reservas!
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Reserva>> GetAllAsync()
        {
            return await _context.Reservas!.ToListAsync();
        }

        public async Task AddAsync(Reserva entity)
        {
            await _context.Reservas!.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Reserva entity)
        {
            _context.Reservas!.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _context.Reservas!.FindAsync(id);
            if (entity != null)
            {
                _context.Reservas.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Reserva>> GetByEstadoAsync(string estado)
        {
            return await _context.Reservas!
                .Where(r => r.EstadoReserva == estado)
                .ToListAsync();
        }

        public async Task<bool> HabitacionDisponibleAsync(Guid habitacionId, DateTime inicio, DateTime fin)
        {
            return !await _context.Reservas!
                .AnyAsync(r =>
                    r.HabitacionId == habitacionId &&
                    (
                        (inicio >= r.FechaInicio && inicio <= r.FechaFin) ||
                        (fin >= r.FechaInicio && fin <= r.FechaFin) ||
                        (inicio <= r.FechaInicio && fin >= r.FechaFin)
                    )
                );
        }
    }
}