using HotelReservation.Application.Contracts;
using HotelReservation.Application.Services;
using HotelReservation.Domain.Interfaces;
using HotelReservation.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HotelReservation.IOC.Modules
{
    public static class RolesIOC
    {
        public static void RegisterRoles(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IRolService, RolService>();
            services.AddScoped<IRolRepository, RolRepository>();
        }
    }
}
