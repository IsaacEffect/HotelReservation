using AutoMapper;
using HotelReservation.Api.Configurations;
using HotelReservation.Application.Base.Mappers;
using HotelReservation.IOC;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace HotelReservation.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // CONFIGURACIÓN GLOBAL DE LOGS

            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            builder.Logging.AddDebug();
            builder.Logging.SetMinimumLevel(LogLevel.Information);

            // CONFIGURACIÓN JWT (usuarios)

            var jwtSettingsSection = builder.Configuration.GetSection("JwtSettings");
            builder.Services.Configure<JwtSettings>(jwtSettingsSection);

            builder.Services.AddSingleton(sp =>
                sp.GetRequiredService<IOptions<JwtSettings>>().Value);


            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var jwtSettings = jwtSettingsSection.Get<JwtSettings>()!;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.Issuer ?? string.Empty,
                        ValidAudience = jwtSettings.Audience ?? string.Empty,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtSettings.SecretKey ?? string.Empty))
                    };
                });

            builder.Services.AddAuthorization();

            // CONTROLLERS

            builder.Services.AddControllers();

            // SWAGGER + JWT

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "HotelReservation API", Version = "v1" });

                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Description = "Ingresa el token JWT con el formato: Bearer {token}",
                    Reference = new OpenApiReference
                    {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme
                    }
                };

                c.AddSecurityDefinition("Bearer", jwtSecurityScheme);

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { jwtSecurityScheme, Array.Empty<string>() }
                });
            });


            // INYECCIÓN DE DEPENDENCIAS (IoC)

            builder.Services.RegisterServices(builder.Configuration);

            // CONFIGURACIÓN DE AUTOMAPPER

            builder.Services.AddSingleton<IMapper>(serviceProvider =>
            {
                var loggerFactory = serviceProvider.GetService<ILoggerFactory>();

                var config = new MapperConfiguration(mapperConfig =>
                {
                    mapperConfig.AddProfile<MappingProfile>();
                }, loggerFactory);

                // Para habilitar validación de mapeos:
                // config.AssertConfigurationIsValid();

                return new Mapper(config);
            });

            // CONSTRUCCIÓN DE LA APP

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // Middlewares de autenticación/autorización
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            // LOG INICIAL DE ARRANQUE

            var logger = app.Services.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("HotelReservation API iniciada correctamente en entorno: {Env}", app.Environment.EnvironmentName);

            app.Run();
        }
    }
}