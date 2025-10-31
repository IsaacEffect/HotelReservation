using System;
using System.Threading.Tasks;

namespace HotelReservation.Application.Interfaces.Services
{
    public interface IReporteService
    {
        Task<decimal> ObtenerIngresosPorRangoAsync(DateTime desde, DateTime hasta);
        Task<int> ObtenerOcupacionPorRangoAsync(DateTime desde, DateTime hasta);
    }
}
