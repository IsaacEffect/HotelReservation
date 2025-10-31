namespace HotelReservation.Application.Contracts
{
    public interface ICheckInOutService
    {
        Task RegistrarCheckInAsync(Guid reservaId);
        Task RegistrarCheckOutAsync(Guid reservaId);
    }
}