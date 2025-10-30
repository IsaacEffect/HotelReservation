using HotelReservation.Domain.Interfaces;
using HotelReservation.Application.DTOs;

namespace HotelReservation.Application.Services
{
    public interface IHistorialService
    {
        Task<IEnumerable<HistorialReservaDto>> GetAllAsync();
        Task<IEnumerable<HistorialReservaDto>> GetByClienteIdAsync(Guid clienteId);
        Task<IEnumerable<HistorialReservaDto>> GetByHabitacionIdAsync(Guid habitacionId);
    }

    public class HistorialService : IHistorialService
    {
        private readonly IHistorialReservaRepository _histRepo;
        public HistorialService(IHistorialReservaRepository histRepo)
        {
            _histRepo = histRepo;
        }

        public async Task<IEnumerable<HistorialReservaDto>> GetAllAsync()
        {
            var ents = await _histRepo.GetAllAsync();
            return ents.Select(e => new HistorialReservaDto
            {
                Id = e.Id,
                HabitacionId = e.HabitacionId,
                ClienteId = e.ClienteId,
                FechaEntrada = e.FechaEntrada,
                FechaSalida = e.FechaSalida,
                Motivo = e.Motivo
            });
        }

        public async Task<IEnumerable<HistorialReservaDto>> GetByClienteIdAsync(Guid clienteId)
        {
            var ents = await _histRepo.GetByClienteIdAsync(clienteId);
            return ents.Select(e => new HistorialReservaDto
            {
                Id = e.Id,
                HabitacionId = e.HabitacionId,
                ClienteId = e.ClienteId,
                FechaEntrada = e.FechaEntrada,
                FechaSalida = e.FechaSalida,
                Motivo = e.Motivo
            });
        }

        public async Task<IEnumerable<HistorialReservaDto>> GetByHabitacionIdAsync(Guid habitacionId)
        {
            var ents = await _histRepo.GetByHabitacionIdAsync(habitacionId);
            return ents.Select(e => new HistorialReservaDto
            {
                Id = e.Id,
                HabitacionId = e.HabitacionId,
                ClienteId = e.ClienteId,
                FechaEntrada = e.FechaEntrada,
                FechaSalida = e.FechaSalida,
                Motivo = e.Motivo
            });
        }
    }

}
