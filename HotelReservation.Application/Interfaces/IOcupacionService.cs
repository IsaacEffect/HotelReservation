using HotelReservation.Application.DTOs;

namespace HotelReservation.Application.Interfaces
{
    public interface IOcupacionService
    {
        Task<OcupacionDiariaDto> ObtenerOcupacionDiariaAsync();
    }

}
