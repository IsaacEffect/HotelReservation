// PRUEBA DE INTEGRACIÓN CORREGIDA
// Para validar la lectura/escritura real usando base de datos en memoria.

using HotelReservation.Domain.Entities;
using HotelReservation.Persistence.Context;
using HotelReservation.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Persistence.Test
{
    public class IntegrationTestUser
    {
        [Fact]
        public async Task AddAndFindUserByEmail_ShouldReturnCorrectUser()
        {
            // Arrange: base de datos única por prueba para evitar contaminación
            var options = new DbContextOptionsBuilder<HotelReservationDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new HotelReservationDBContext(options);
            var repo = new UsuarioRepository(context);

            // 1. Crear e Insertar la Entidad 'Rol' para satisfacer el .Include()
            var rolId = Guid.NewGuid();
            var rol = new Rol
            {
                RolId = rolId,
                NombreRol = "Cliente",
            };

            await context.Roles.AddAsync(rol);

            // 2. Crear la Entidad 'Usuario' usando el RolId existente
            var usuario = new Usuario
            {
                IdUsuario = Guid.NewGuid(),
                Nombre = "Robinson",
                Apellido = "Cano",
                Correo = "robinsoncano@gmail.com",
                Contrasena = "password123",
                RolId = rolId,
                Estado = true
            };

            // Act: agregar usuario y guardar
            await repo.AddAsync(usuario);
            await context.SaveChangesAsync(); // Se guardan el Rol y el Usuario

            // Buscar el usuario usando el repositorio
            var found = await repo.GetByEmailAsync("robinsoncano@gmail.com");

            // Assert
            Assert.NotNull(found);
            Assert.Equal("robinsoncano@gmail.com", found!.Correo);

            // verificar que el include funcionó
            Assert.NotNull(found.Rol);
            Assert.Equal(rolId, found.Rol.RolId);
        }
    }
}