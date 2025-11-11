using HotelReservation.Domain.Entities;

namespace HotelReservation.Domain.Interfaces
{
    public interface IHistorialReservaRepository
    {
        Task<HistorialReserva> AddAsync(HistorialReserva entity);
        Task<IEnumerable<HistorialReserva>> GetAllAsync();
        Task<IEnumerable<HistorialReserva>> GetByClienteIdAsync(Guid clienteId);
        Task<IEnumerable<HistorialReserva>> GetByHabitacionIdAsync(Guid habitacionId);
        Task<HistorialReserva?> GetByClienteYFechasAsync(Guid clienteId, DateTime fechaEntrada, DateTime fechaSalida);
    }

}