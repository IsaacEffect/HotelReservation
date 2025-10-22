using HotelReservation.Domain.Interfaces;
using HotelReservation.Persistence.Context;

namespace HotelReservation.Persistence.Base
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly HotelReservationDBContext _context;

        public IClienteRepository Clientes { get; }
        public IUsuarioRepository Usuarios { get; }
        public IRolRepository Roles { get; }

        public UnitOfWork(
            HotelReservationDBContext context,
            IClienteRepository clienteRepository,
            IUsuarioRepository usuarioRepository,
            IRolRepository rolRepository)
        {
            _context = context;
            Clientes = clienteRepository;
            Usuarios = usuarioRepository;
            Roles = rolRepository;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

    }
}
