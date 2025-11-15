using HotelReservation.Application.Contracts;
using HotelReservation.Application.Services;
using HotelReservation.Domain.Interfaces;
using HotelReservation.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HotelReservation.IOC.Modules
{
    public static class CategoriaIOC
    {
        public static void RegisterCategorias(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ICategoriaHabitacionService, CategoriaHabitacionService>();
            services.AddScoped<ICategoriaHabitacionRepository, CategoriaHabitacionRepository>();
        }
    }
}