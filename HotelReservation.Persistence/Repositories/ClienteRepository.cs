using Microsoft.EntityFrameworkCore;
using HotelReservation.Domain.Entities;
using HotelReservation.Domain.Interfaces;
using HotelReservation.Persistence.Context;

namespace HotelReservation.Persistence.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly HotelReservationDbContext _context;
        public ClienteRepository(HotelReservationDbContext context)
        {
            _context = context;
        }

        public async Task<Cliente?> GetByIdAsync(Guid id)
        {
            return await _context.Clientes!.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Cliente>> GetAllAsync()
        {
            return await _context.Clientes!.ToListAsync();
        }

        public async Task<Cliente> AddAsync(Cliente entity)
        {
            _context.Clientes!.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Cliente> UpdateAsync(Cliente entity)
        {
            _context.Clientes!.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _context.Clientes!.FindAsync(id);
            if (entity != null)
            {
                _context.Clientes!.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
