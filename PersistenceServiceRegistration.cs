using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using HotelReservation.Persistence.Contexts;

namespace HotelReservation.Persistence
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<HotelReservationContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("HotelReservationDB")));

            return services;
        }
    }
}