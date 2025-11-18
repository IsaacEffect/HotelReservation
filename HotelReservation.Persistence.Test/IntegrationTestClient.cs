// PRUEBA DE INTEGRACIÓN
// Aca se usa EF Core InMemory para simular una BD real y probar operaciones completas.

using HotelReservation.Domain.Entities;
using HotelReservation.Persistence.Context;
using HotelReservation.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Persistence.Test
{
    public class IntegrationTestClient
    {
        [Fact]
        public async Task AddAndRetrieveClient_ShouldWorkCorrectly()
        {
            var options = new DbContextOptionsBuilder<HotelReservationDBContext>()
                .UseInMemoryDatabase("ClientDB_IntegrationTest")
                .Options;

            using var context = new HotelReservationDBContext(options);
            var repository = new ClienteRepository(context);

            var cliente = new Cliente { Nombre = "Manny", Apellido = "Ramirez", Correo = "mramirez@gmail.com" };
            await repository.AddAsync(cliente);
            await context.SaveChangesAsync();

            var result = await repository.GetAllAsync();

            Assert.Contains(result, c => c.Correo == "mramirez@gmail.com");
        }

        [Fact]
        public async Task DeleteClient_ShouldSetEstadoFalse()
        {
            var options = new DbContextOptionsBuilder<HotelReservationDBContext>()
                .UseInMemoryDatabase("ClientDeleteTest")
                .Options;

            using var context = new HotelReservationDBContext(options);
            var repository = new ClienteRepository(context);

            var cliente = new Cliente { Nombre = "Bartolo", Apellido = "Colon", Correo = "bcolon@gmail.com" };
            await repository.AddAsync(cliente);
            await context.SaveChangesAsync();

            await repository.DeleteAsync(cliente.IdCliente);
            await context.SaveChangesAsync();

            var result = await repository.GetByIdAsync(cliente.IdCliente);
            Assert.False(result.Estado);
        }
    }
}
