using HotelReservation.Application.Services;
using HotelReservation.Domain.Interfaces;
using HotelReservation.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HotelReservation.IOC.Modules
{
    public static class HistorialIOC
    {
        public static void RegisterHistorial(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IHistorialReservaRepository, HistorialReservaRepository>();
            services.AddScoped<IHistorialService, HistorialService>();
        }
    }
}
