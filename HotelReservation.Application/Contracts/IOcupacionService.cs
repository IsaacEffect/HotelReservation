using HotelReservation.Application.DTOs;

namespace HotelReservation.Application.Contracts
{
    public interface IOcupacionService
    {
        Task<OcupacionDiariaDto> ObtenerOcupacionDiariaAsync();
    }

}