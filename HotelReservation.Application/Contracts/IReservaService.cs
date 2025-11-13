using HotelReservation.Application.Dtos;
using HotelReservation.Domain.Entities;

namespace HotelReservation.Application.Contracts
{
    public interface IReservaService
    {
        Task<Reserva?> GetByIdAsync(Guid id);
        Task<IEnumerable<Reserva>> GetAllAsync();
        Task<Reserva> AddAsync(Reserva entity);
        Task<Reserva> UpdateAsync(Reserva entity);
        Task DeleteAsync(Guid id);

        // ------------------------------------------------------
        Task<Guid> CrearReservaAsync(CrearReservaDTO dto);
        Task<IEnumerable<Reserva>> ObtenerReservasAsync();
        Task<IEnumerable<object>> ObtenerReservasConDetallesAsync();
    }
}