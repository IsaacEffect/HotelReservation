namespace HotelReservation.Application.Contracts
{
    public interface IReporteService
    {
        Task<decimal> ObtenerIngresosPorRangoAsync(DateTime desde, DateTime hasta);
        Task<int> ObtenerOcupacionPorRangoAsync(DateTime desde, DateTime hasta);
    }
}
