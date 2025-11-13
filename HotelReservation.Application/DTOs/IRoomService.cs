using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelReservation.Application.DTOs
{
    public interface IRoomService
    {
        Task<IEnumerable<RoomDto>> GetAllAsync();
        Task<RoomDto?> GetByIdAsync(int id);
        Task<RoomDto> AddAsync(RoomDto dto);
        Task UpdateAsync(RoomDto dto);
        Task DeleteAsync(int id);
    }
}