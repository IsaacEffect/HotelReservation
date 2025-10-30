using HotelReservation.Domain.Entities;
using HotelReservation.Domain.Interfaces;
using HotelReservation.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Persistence.Repositories
{
    public class HistorialReservaRepository : IHistorialReservaRepository
    {
        private readonly HotelReservationDbContext _context;
        public HistorialReservaRepository(HotelReservationDbContext context)
        {
            _context = context;
        }

        public async Task<HistorialReserva> AddAsync(HistorialReserva entity)
        {
            _context.HistorialReservas.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<IEnumerable<HistorialReserva>> GetAllAsync()
        {
            return await _context.HistorialReservas.OrderByDescending(x => x.FechaEntrada).ToListAsync();
        }

        public async Task<IEnumerable<HistorialReserva>> GetByClienteIdAsync(Guid clienteId)
        {
            return await _context.HistorialReservas.Where(x => x.ClienteId == clienteId)
                                                  .OrderByDescending(x => x.FechaEntrada)
                                                  .ToListAsync();
        }

        public async Task<IEnumerable<HistorialReserva>> GetByHabitacionIdAsync(Guid habitacionId)
        {
            return await _context.HistorialReservas.Where(x => x.HabitacionId == habitacionId)
                                                  .OrderByDescending(x => x.FechaEntrada)
                                                  .ToListAsync();
        }

        public async Task<HistorialReserva?> GetByClienteYFechasAsync(Guid clienteId, DateTime fechaEntrada, DateTime fechaSalida)
        {
            return await _context.HistorialReservas
                .FirstOrDefaultAsync(h =>
                    h.ClienteId == clienteId &&
                    h.FechaEntrada == fechaEntrada &&
                    h.FechaSalida == fechaSalida);
        }
    }
}
