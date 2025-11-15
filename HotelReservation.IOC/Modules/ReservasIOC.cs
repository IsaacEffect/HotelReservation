using HotelReservation.Application.Contracts;
using HotelReservation.Application.Services;
using HotelReservation.Domain.Interfaces;
using HotelReservation.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HotelReservation.IOC.Modules
{
    public static class ReservasIOC
    {
        public static void RegisterReservas(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IReservaRepository, ReservaRepository>();
            services.AddScoped<IReservaService, ReservaService>();
        }
    }
}
