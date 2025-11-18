using HotelReservation.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HotelReservation.IOC.Modules
{
    public static class DbContextIOC
    {
        public static void RegisterDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("HotelDBConnection");

            services.AddDbContext<HotelReservationDBContext>(options =>
                options.UseSqlServer(connectionString));
        }
    }
}
