using HotelReservation.Application.Dtos;

namespace HotelReservation.Application.Contracts
{
    public interface IOcupacionService
    {
        Task<OcupacionDiariaDto> ObtenerOcupacionDiariaAsync();
    }

}