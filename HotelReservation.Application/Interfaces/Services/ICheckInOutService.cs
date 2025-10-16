using System;
using System.Threading.Tasks;

namespace HotelReservation.Application.Interfaces.Services
{
    public interface ICheckInOutService
    {
        Task RegistrarCheckInAsync(Guid reservaId);
        Task RegistrarCheckOutAsync(Guid reservaId);
    }
}