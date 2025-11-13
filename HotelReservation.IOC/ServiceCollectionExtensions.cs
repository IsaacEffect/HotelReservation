using HotelReservation.Application.Contracts;
using HotelReservation.Application.Services;
using HotelReservation.Domain.Interfaces;
using HotelReservation.Persistence.Context;
using HotelReservation.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace HotelReservation.IOC
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHotelReservationPersistence(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<HotelReservationDBContext>(options =>
                options.UseSqlServer(connectionString));

            // Repositories
            services.AddScoped<ICheckInOutRepository, CheckInOutRepository>();
            services.AddScoped<IHistorialReservaRepository, HistorialReservaRepository>();
            // Facturación
            services.AddScoped<IFacturaService, FacturaService>();

            //services.AddScoped<IReservaRepository, ReservaRepository>();
            services.AddScoped<IHabitacionRepository, HabitacionRepository>();
            services.AddScoped<IClienteRepository, ClienteRepository>();
            services.AddScoped<IOcupacionService, OcupacionService>();

            // Services
            services.AddScoped<ICheckInOutService, CheckInOutService>();
            services.AddScoped<IHistorialService, HistorialService>();

            return services;
        }
    }
}