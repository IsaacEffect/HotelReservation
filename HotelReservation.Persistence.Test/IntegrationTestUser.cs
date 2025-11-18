// PRUEBA DE INTEGRACIÓN
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
            var options = new DbContextOptionsBuilder<HotelReservationDBContext>()
                .UseInMemoryDatabase("UserDB_IntegrationTest")
                .Options;

            using var context = new HotelReservationDBContext(options);
            var repository = new UsuarioRepository(context);

            var usuario = new Usuario
            {
                Nombre = "Robinson",
                Apellido = "Cano",
                Correo = "robinsoncano@gmail.com",
                Contrasena = "password123",
                RolId = Guid.NewGuid()
            };

            await repository.AddAsync(usuario);
            await context.SaveChangesAsync();

            var found = await repository.GetByEmailAsync("robinsoncano@gmail.com");

            Assert.NotNull(found);
            Assert.Equal("robinsoncano@gmail.com", found.Correo);
        }
    }
}
