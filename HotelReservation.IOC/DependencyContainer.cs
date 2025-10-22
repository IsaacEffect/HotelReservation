using HotelReservation.Domain.Interfaces;
using HotelReservation.IOC.Modules;
using HotelReservation.Persistence.Base;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HotelReservation.IOC
{
    public static class DependencyContainer
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.RegisterClientes(configuration);
            services.RegisterUsuarios(configuration);
            services.RegisterRoles(configuration);
            services.RegisterDbContext(configuration);
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddLogging();
        }
    }
}
