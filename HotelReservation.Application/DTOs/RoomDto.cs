using HotelReservation.Domain.Entities;

namespace HotelReservation.Application.DTOs
{
    public record RoomDto(Guid Id, int Number, Guid CategoryId, RoomStatus Status, decimal Price);
}
