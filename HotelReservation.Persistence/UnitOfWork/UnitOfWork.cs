using System.Threading.Tasks;
using HotelReservation.Domain.Interfaces;
using HotelReservation.Persistence.Context;
using HotelReservation.Persistence.Repositories;

namespace HotelReservation.Persistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly HotelReservationDBContext _context;
        private IFacturaRepository _facturaRepository;

        public UnitOfWork(HotelReservationDBContext context)
        {
            _context = context;
        }

        public IFacturaRepository Facturas => _facturaRepository ??= new FacturaRepository(_context);

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}

