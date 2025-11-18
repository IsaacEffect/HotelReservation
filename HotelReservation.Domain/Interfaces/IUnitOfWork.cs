namespace HotelReservation.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IClienteRepository Clientes { get; }
        IUsuarioRepository Usuarios { get; }
        IRolRepository Roles { get; }
        ICategoriaHabitacionRepository Categorias { get; }
        IReservaRepository Reservas { get; }
        Task<int> SaveChangesAsync();
    }
}
