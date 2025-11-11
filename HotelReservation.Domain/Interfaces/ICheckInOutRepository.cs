using HotelReservation.Domain.Entities;

namespace HotelReservation.Domain.Interfaces
{
    public interface ICheckInOutRepository
    {
        Task<CheckInOut?> GetByReservaIdAsync(Guid reservaId);
        Task<CheckInOut> AddAsync(CheckInOut entity);
        Task<CheckInOut> UpdateAsync(CheckInOut entity);
        Task DeleteAsync(Guid id);
    }

}