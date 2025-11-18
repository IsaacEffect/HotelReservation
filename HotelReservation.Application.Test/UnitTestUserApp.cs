//// PRUEBA UNITARIA
//// Para verificar el comportamiento del servicio sin usar base de datos real.

//using HotelReservation.Application.Services;
//using HotelReservation.Domain.Entities;
//using HotelReservation.Domain.Interfaces;
//using Moq;

//namespace HotelReservation.Application.Test
//{
//    public class UnitTestUserApp
//    {
//        [Fact]
//        public async Task GetUserByEmail_ShouldReturnUsuario()
//        {
//            var expectedUser = new Usuario { Correo = "sambysosa@gmail.com" };
//            var mockUow = new Mock<IUnitOfWork>();
//            mockUow.Setup(u => u.Usuarios.GetByEmailAsync("sambysosa@gmail.com"))
//                   .ReturnsAsync(expectedUser);

//            var service = new UsuarioService(mockUow.Object);

//            var result = await service.GetByEmailAsync("sambysosa@gmail.com");

//            Assert.NotNull(result);
//            Assert.Equal("sambysosa@gmail.com", result.Correo);
//        }

//        [Fact]
//        public async Task AddUser_ShouldCallRepositoryAndSave()
//        {
//            var mockRepo = new Mock<IUsuarioRepository>();
//            var mockUoW = new Mock<IUnitOfWork>();
//            mockUoW.Setup(u => u.Usuarios).Returns(mockRepo.Object);

//            var service = new UsuarioService(mockUoW.Object);
//            var usuario = new Usuario { Nombre = "Vladimir", Apellido = "Guerrero", Correo = "vladguerrero@gmail.com", Contrasena = "clave123" };

//            await service.AddAsync(usuario);

//            mockRepo.Verify(r => r.AddAsync(It.IsAny<Usuario>()), Times.Once);
//            mockUoW.Verify(u => u.SaveChangesAsync(), Times.Once);
//        }
//    }
//}
