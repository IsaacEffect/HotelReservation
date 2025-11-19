using HotelReservation.Application.Contracts;
using HotelReservation.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Application.Services
{
    public class ReporteService : IReporteService
    {
        private readonly HotelReservationDBContext _context;

        public ReporteService(HotelReservationDBContext context)
        {
            _context = context;
        }

        public async Task<decimal> ObtenerIngresosPorRangoAsync(DateTime desde, DateTime hasta)
        {
            return await _context.Facturas
                .Where(f => f.FechaEmision >= desde && f.FechaEmision <= hasta)
                .SumAsync(f => f.MontoTotal);
        }

        public async Task<int> ObtenerOcupacionPorRangoAsync(DateTime desde, DateTime hasta)
        {
            return await _context.Reservas
                .Where(r => r.FechaInicio <= hasta && r.FechaFin >= desde)
                .CountAsync();
        }
    }
}