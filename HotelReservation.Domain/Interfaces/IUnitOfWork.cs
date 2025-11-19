namespace HotelReservation.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IClienteRepository Clientes { get; }
        IUsuarioRepository Usuarios { get; }
        IRolRepository Roles { get; }
        ICategoriaHabitacionRepository Categorias { get; }
        IFacturaRepository Facturas { get; }
        Task<int> SaveChangesAsync();
    }
}
