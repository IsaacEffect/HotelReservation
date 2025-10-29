using HotelReservation.Persistence.Repositories;
using HotelReservation.Application.Services;

namespace HotelReservation.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1️⃣ Agregar controladores y Swagger
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // 2️⃣ Registrar las dependencias del proyecto (inyección de dependencias)
            builder.Services.AddScoped<ReservaRepository>();
            builder.Services.AddScoped<ReservaService>();

            var app = builder.Build();

            // 3️⃣ Configurar el pipeline HTTP
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();

            // 4️⃣ Mapear los controladores (endpoints)
            app.MapControllers();

            app.Run();
        }
    }
}
