using HotelReservation.Application.Dtos;

namespace HotelReservation.Application.Contracts
{
    public interface IReservaService
    {
        Task<ReservaDetalleDTO?> GetByIdAsync(Guid id);
        Task<IEnumerable<ReservaDetalleDTO>> GetAllAsync();
        Task<Guid> CrearReservaAsync(CrearReservaDTO dto);
        Task<bool> VerificarDisponibilidadAsync(Guid habitacionId, DateTime fechaInicio, DateTime fechaFin);
        Task ActualizarReservaAsync(ActualizarReservaDTO dto);

        // Cancelar Reserva (Llamará a CambiarEstado con "Cancelada")
        Task CancelarReservaAsync(Guid reservaId);
        Task CambiarEstadoReservaAsync(ActualizarEstadoReservaDTO dto);
        Task<IEnumerable<ReservaDetalleDTO>> ObtenerReservasConDetallesAsync();
    }
}