using HotelReservation.Application.Contracts;
using HotelReservation.Application.Dtos;
using HotelReservation.Domain.Interfaces;

namespace HotelReservation.Application.Services
{
    public class ReservaService : IReservaService
    {
        private readonly IReservaRepository _repo;

        public ReservaService(IReservaRepository repo)
        {
            _repo = repo;
        }

        Task<IEnumerable<ReservaDTO>> IReservaService.GetAllAsync()
        {
            throw new NotImplementedException();
        }

        Task<ReservaDTO?> IReservaService.GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task ActualizarReservaAsync(ActualizarReservaDTO dto)
        {
            throw new NotImplementedException();
        }

        public Task CambiarEstadoReservaAsync(ActualizarEstadoReservaDTO dto)
        {
            throw new NotImplementedException();
        }

        public Task CancelarReservaAsync(Guid reservaId)
        {
            throw new NotImplementedException();
        }

        public Task<Guid> CrearReservaAsync(CrearReservaDTO dto)
        {
            throw new NotImplementedException();
        }


        public Task<ReservaDetalleDTO?> GetReservaDetalleByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ReservaDetalleDTO>> ObtenerReservasConDetallesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> VerificarDisponibilidadAsync(Guid habitacionId, DateTime fechaInicio, DateTime fechaFin)
        {
            throw new NotImplementedException();
        }
    }
}