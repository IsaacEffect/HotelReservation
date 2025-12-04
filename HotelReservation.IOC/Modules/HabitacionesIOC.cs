using HotelReservation.Application.Contracts;
using HotelReservation.Application.Services;
using HotelReservation.Domain.Interfaces;
using HotelReservation.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HotelReservation.IOC.Modules
{
    public static class HabitacionesIOC
    {
        public static void RegisterHabitaciones(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IHabitacionRepository, HabitacionRepository>();
            services.AddScoped<IHabitacionService, HabitacionService>();
        }
    }
}
