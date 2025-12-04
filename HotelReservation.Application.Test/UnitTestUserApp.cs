using AutoMapper;
using HotelReservation.Application.Dtos;
using HotelReservation.Application.Services;
using HotelReservation.Domain.Entities;
using HotelReservation.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace HotelReservation.Application.Test
{
    public class UnitTestUserApp
    {
        private readonly Mock<IUnitOfWork> _mockUoW;
        private readonly Mock<IUsuarioRepository> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<UsuarioService>> _mockLogger;
        private readonly UsuarioService _service;

        public UnitTestUserApp()
        {
            _mockUoW = new Mock<IUnitOfWork>();
            _mockRepo = new Mock<IUsuarioRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<UsuarioService>>();

            _mockUoW.Setup(u => u.Usuarios).Returns(_mockRepo.Object);

            _service = new UsuarioService(_mockUoW.Object, _mockMapper.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetUserByEmail_ShouldReturnUsuario()
        {
            var usuario = new Usuario { Correo = "sambysosa@gmail.com" };

            _mockRepo.Setup(r => r.GetByEmailAsync("sambysosa@gmail.com"))
                     .ReturnsAsync(usuario);

            _mockMapper.Setup(m => m.Map<ObtenerUsuarioDto>(usuario))
                       .Returns(new ObtenerUsuarioDto { Correo = "sambysosa@gmail.com" });

            var result = await _service.GetByEmailAsync("sambysosa@gmail.com");

            Assert.NotNull(result);
            Assert.Equal("sambysosa@gmail.com", result.Correo);
        }

        [Fact]
        public async Task AddUser_ShouldCallRepositoryAndSave()
        {
            var dto = new InsertarUsuarioDto
            {
                Nombre = "Vladimir",
                Apellido = "Guerrero",
                Correo = "vladguerrero@gmail.com",
                Contrasena = "clave123"
            };

            var usuario = new Usuario();

            _mockMapper.Setup(m => m.Map<Usuario>(dto)).Returns(usuario);

            await _service.AddAsync(dto);

            _mockRepo.Verify(r => r.AddAsync(It.IsAny<Usuario>()), Times.Once);
            _mockUoW.Verify(u => u.SaveChangesAsync(), Times.Once);
        }
    }
}
