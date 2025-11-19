using HotelReservation.Application.Dtos;
<<<<<<< HEAD
using HotelReservation.Domain.Entities;
=======
>>>>>>> origin/develop

namespace HotelReservation.Application.Contracts
{
    public interface IReservaService
    {
<<<<<<< HEAD
        Task<Reserva?> GetByIdAsync(Guid id);
        Task<IEnumerable<Reserva>> GetAllAsync();
        Task<Reserva> AddAsync(Reserva entity);
        Task<Reserva> UpdateAsync(Reserva entity);
        Task DeleteAsync(Guid id);

        // ------------------------------------------------------
        Task<Guid> CrearReservaAsync(CrearReservaDTO dto);
        Task<IEnumerable<Reserva>> ObtenerReservasAsync();
        Task<IEnumerable<object>> ObtenerReservasConDetallesAsync();
=======
        Task<ReservaDTO?> GetByIdAsync(Guid id);
        Task<IEnumerable<ReservaDTO>> GetAllAsync();
        Task<Guid> CrearReservaAsync(CrearReservaDTO dto);
        Task<bool> VerificarDisponibilidadAsync(Guid habitacionId, DateTime fechaInicio, DateTime fechaFin);
        Task ActualizarReservaAsync(ActualizarReservaDTO dto);

        // Cancelar Reserva (Llamará a CambiarEstado con "Cancelada")
        Task CancelarReservaAsync(Guid reservaId);
        Task CambiarEstadoReservaAsync(ActualizarEstadoReservaDTO dto);
        Task<IEnumerable<ReservaDetalleDTO>> ObtenerReservasConDetallesAsync();
        Task<ReservaDetalleDTO?> GetReservaDetalleByIdAsync(Guid id);
>>>>>>> origin/develop
    }
}