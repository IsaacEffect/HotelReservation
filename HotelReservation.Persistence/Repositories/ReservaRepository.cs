using HotelReservation.Domain.Entities;
using HotelReservation.Domain.Interfaces;

namespace HotelReservation.Persistence.Repositories
{
    public class ReservaRepository : IReservaRepository
    {
        public Task AddAsync(Reserva entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Reserva>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Reserva>> GetByEstadoAsync(string estado)
        {
            throw new NotImplementedException();
        }

        public Task<Reserva?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HabitacionDisponibleAsync(Guid habitacionId, DateTime fechaInicio, DateTime fechaFin)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<object>> ObtenerReservasConDetallesAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Reserva entity)
        {
            throw new NotImplementedException();
        }
    }
}