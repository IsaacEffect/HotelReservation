//// PRUEBA UNITARIA
//// Para verificar el comportamiento del servicio sin usar base de datos real.

//using HotelReservation.Application.Services;
//using HotelReservation.Domain.Entities;
//using HotelReservation.Domain.Interfaces;
//using Moq;

//namespace HotelReservation.Application.Test
//{
//    public class UnitTestClientApp
//    {
//        [Fact]
//        public async Task AddClient_ShouldCallRepositoryAndSave()
//        {
//            // Arrange
//            var mockRepo = new Mock<IClienteRepository>();
//            var mockUoW = new Mock<IUnitOfWork>();
//            mockUoW.Setup(u => u.Clientes).Returns(mockRepo.Object);

//            var service = new ClienteService(mockUoW.Object);
//            var cliente = new Cliente { Nombre = "David", Apellido = "Ortiz", Correo = "davidortiz@gmail.com" };

//            // Act
//            await service.AddAsync(cliente);

//            // Assert
//            mockRepo.Verify(r => r.AddAsync(It.IsAny<Cliente>()), Times.Once);
//            mockUoW.Verify(u => u.SaveChangesAsync(), Times.Once);
//        }

//        [Fact]
//        public async Task DeleteClient_ShouldCallDeleteAndSave()
//        {
//            var mockRepo = new Mock<IClienteRepository>();
//            var mockUoW = new Mock<IUnitOfWork>();
//            mockUoW.Setup(u => u.Clientes).Returns(mockRepo.Object);

//            var service = new ClienteService(mockUoW.Object);
//            var id = Guid.NewGuid();

//            await service.DeleteAsync(id);

//            mockRepo.Verify(r => r.DeleteAsync(id), Times.Once);
//            mockUoW.Verify(u => u.SaveChangesAsync(), Times.Once);
//        }

//        [Fact]
//        public async Task GetAllClients_ShouldReturnList()
//        {
//            var mockRepo = new Mock<IClienteRepository>();
//            mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Cliente>
//            {
//                new() { Nombre = "Albert", Apellido = "Pujols", Correo = "pujols@gmail.com" }
//            });

//            var mockUoW = new Mock<IUnitOfWork>();
//            mockUoW.Setup(u => u.Clientes).Returns(mockRepo.Object);

//            var service = new ClienteService(mockUoW.Object);

//            var result = await service.GetAllAsync();

//            Assert.NotEmpty(result);
//        }
//    }
//}
