using HotelReservation.Application.Contracts;
using HotelReservation.Application.Dtos;
using HotelReservation.Domain.Entities;
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

        // Crear una nueva reserva
        public async Task<Guid> CrearReservaAsync(CrearReservaDTO dto)
        {
            if (dto.FechaInicio >= dto.FechaFin)
                throw new Exception("Las fechas de inicio y fin son inválidas.");

            // Verificar disponibilidad
            if (!await _repo.HabitacionDisponibleAsync(dto.HabitacionId, dto.FechaInicio, dto.FechaFin))
                throw new Exception("La habitación no está disponible en ese rango de fechas.");

            var reserva = new Reserva
            {
                Id = Guid.NewGuid(),
                FechaInicio = dto.FechaInicio,
                FechaFin = dto.FechaFin,
                EstadoReserva = "Pendiente",
                ClienteId = dto.ClienteId,
                HabitacionId = dto.HabitacionId,
                UsuarioId = dto.UsuarioId,
                Total = 0m // Inicialmente 0, se actualizará después con el cálculo correspondiente
            };

            // Crear la reserva
            return await _repo.CrearReservaAsync(reserva);
        }

        // Obtener todas las reservas
        public Task<IEnumerable<Reserva>> ObtenerReservasAsync()
        {
            return _repo.ObtenerReservasAsync();
        }

        // Obtener reservas con información detallada
        public Task<IEnumerable<object>> ObtenerReservasConDetallesAsync()
        {
            return _repo.ObtenerReservasConDetallesAsync();
        }

        // ----------------------------------------------------------------------

        public Task<Reserva?> GetByIdAsync(Guid id)
        {
            return _repo.GetByIdAsync(id);
        }

        public Task<IEnumerable<Reserva>> GetAllAsync()
        {
            return _repo.GetAllAsync();
        }

        public Task<Reserva> AddAsync(Reserva entity)
        {
            return _repo.AddAsync(entity);
        }

        public Task<Reserva> UpdateAsync(Reserva entity)
        {
            return _repo.UpdateAsync(entity);
        }

        public Task DeleteAsync(Guid id)
        {
            return _repo.DeleteAsync(id);
        }
    }
}