using HotelReservation.Application.Contracts;
using HotelReservation.Application.Services;
using HotelReservation.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HotelReservation.IOC.Modules
{
    public static class FacturacionIOC
    {
        public static void RegisterFacturacion(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IFacturaService, FacturaService>();
            services.AddScoped<IReporteService, ReporteService>();
            services.AddSingleton<IPdfService, PdfService>();
        }
    }
}
