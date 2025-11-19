using HotelReservation.Domain.Entities;

<<<<<<< HEAD
=======

>>>>>>> origin/develop
namespace HotelReservation.Domain.Interfaces
{
    public interface IReservaRepository
    {
        Task<Reserva?> GetByIdAsync(Guid id);
        Task<IEnumerable<Reserva>> GetAllAsync();
<<<<<<< HEAD
        Task<Reserva> AddAsync(Reserva entity);
        Task<Reserva> UpdateAsync(Reserva entity);
        Task DeleteAsync(Guid id);

        // --------------------------------------------------
        Task<bool> HabitacionDisponibleAsync(Guid habitacionId, DateTime fechaInicio, DateTime fechaFin);
        Task<Guid> CrearReservaAsync(Reserva reserva);
        Task<IEnumerable<Reserva>> ObtenerReservasAsync();
        Task<IEnumerable<object>> ObtenerReservasConDetallesAsync();
=======
        Task AddAsync(Reserva entity);
        Task UpdateAsync(Reserva entity);
        Task DeleteAsync(Guid id);

        Task<bool> HabitacionDisponibleAsync(Guid habitacionId, DateTime fechaInicio, DateTime fechaFin);
        Task<IEnumerable<Reserva>> GetByEstadoAsync(string estado);
>>>>>>> origin/develop
    }
}