using HotelReservation.Domain.Entities;
using HotelReservation.Domain.Interfaces;
using HotelReservation.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Persistence.Repositories
{
    public class FacturaRepository : IFacturaRepository
    {
        private readonly HotelReservationDBContext _context;

        public FacturaRepository(HotelReservationDBContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Factura factura)
        {
            await _context.Facturas.AddAsync(factura);
        }

        public async Task AddDetalleAsync(DetalleFactura detalle)
        {
            await _context.DetalleFactura.AddAsync(detalle);
        }

        public async Task<IEnumerable<Factura>> GetAllAsync()
        {
            return await _context.Facturas
                .Include(f => f.Detalles)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Factura?> GetByIdAsync(Guid id)
        {
            return await _context.Facturas
                .Include(f => f.Detalles)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<bool> ExistsByReservaIdAsync(Guid reservaId)
        {
            return await _context.Facturas.AsNoTracking().AnyAsync(f => f.ReservaId == reservaId);
        }
    }
}