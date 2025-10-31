namespace HotelReservation.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IClienteRepository Clientes { get; }
        IUsuarioRepository Usuarios { get; }
        IRolRepository Roles { get; }
        Task<int> SaveChangesAsync();
    }
}
