using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using HotelReservation.Persistence.Context;

namespace HotelReservation.Persistence.Base
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<HotelReservationDBContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("HotelDBConnection")));

            return services;
        }
    }
}