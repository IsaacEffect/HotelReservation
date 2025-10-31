using HotelReservation.Domain.Entities;
using HotelReservation.Domain.Interfaces;
using HotelReservation.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Persistence.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly HotelReservationDBContext _context;

        public ClienteRepository(HotelReservationDBContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Cliente cliente)
        {
            if (cliente == null) throw new ArgumentNullException(nameof(cliente));

            if (string.IsNullOrWhiteSpace(cliente.Nombre))
                throw new ArgumentException("El nombre es requerido", nameof(cliente.Nombre));

            await _context.Clientes.AddAsync(cliente);
        }

        public async Task DeleteAsync(Guid id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente != null)
            {
                cliente.Estado = false;
                _context.Clientes.Update(cliente);
            }
        }

        public async Task<IEnumerable<Cliente>> GetAllAsync()
        {
            return await _context.Clientes.ToListAsync();
        }

        public async Task<Cliente> GetByIdAsync(Guid id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
                throw new KeyNotFoundException($"Cliente con id {id} no encontrado.");

            return cliente;
        }

        public async Task UpdateAsync(Cliente cliente)
        {
            if (cliente == null) throw new ArgumentNullException(nameof(cliente));
            _context.Clientes.Update(cliente);
            await Task.CompletedTask;
        }

        public async Task<Cliente?> GetByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return null;

            var normalizedEmail = email.Trim().ToLowerInvariant();

            return await _context.Clientes
                .AsNoTracking()
                .FirstOrDefaultAsync(c =>
                    c.Estado &&
                    c.Correo != null &&
                    c.Correo.Trim().ToLowerInvariant() == normalizedEmail);
        }

    }
}
