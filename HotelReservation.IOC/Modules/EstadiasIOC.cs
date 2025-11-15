using HotelReservation.Application.Contracts;
using HotelReservation.Application.Services;
using HotelReservation.Domain.Interfaces;
using HotelReservation.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HotelReservation.IOC.Modules
{
    public static class EstadiasIOC
    {
        public static void RegisterCheckInOut(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ICheckInOutRepository, CheckInOutRepository>();
            services.AddScoped<ICheckInOutService, CheckInOutService>();
            services.AddScoped<IOcupacionService, OcupacionService>();
        }
    }
}
