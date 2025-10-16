using Microsoft.Extensions.DependencyInjection;
using HotelReservation.Application.Interfaces.Services;
using HotelReservation.Application.Services;

namespace HotelReservation.IOC
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<ICheckInOutService, CheckInOutService>();
            services.AddScoped<IReporteService, ReporteService>();
            return services;
        }
    }
}