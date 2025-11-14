using HotelReservation.Domain.Entities;

namespace HotelReservation.Application.DTOs
{
    public record HabitacionDto(Guid Id, int Number, Guid CategoryId, HabitacionStatus Status, decimal Price);
}