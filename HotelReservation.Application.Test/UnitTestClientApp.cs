using AutoMapper;
using HotelReservation.Application.Dtos;
using HotelReservation.Application.Services;
using HotelReservation.Domain.Entities;
using HotelReservation.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace HotelReservation.Application.Test
{
    public class UnitTestClientApp
    {
        private readonly Mock<IUnitOfWork> _mockUoW;
        private readonly Mock<IClienteRepository> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<ClienteService>> _mockLogger;
        private readonly ClienteService _service;

        public UnitTestClientApp()
        {
            _mockUoW = new Mock<IUnitOfWork>();
            _mockRepo = new Mock<IClienteRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<ClienteService>>();

            _mockUoW.Setup(u => u.Clientes).Returns(_mockRepo.Object);

            _service = new ClienteService(_mockUoW.Object, _mockMapper.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task AddClient_ShouldCallRepositoryAndSave()
        {
            var dto = new InsertarClienteDto { Nombre = "David" };
            var cliente = new Cliente { Nombre = "David" };

            _mockMapper.Setup(m => m.Map<Cliente>(dto)).Returns(cliente);

            await _service.AddAsync(dto);

            _mockRepo.Verify(r => r.AddAsync(It.IsAny<Cliente>()), Times.Once);
            _mockUoW.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteClient_ShouldCallDeleteAndSave()
        {
            var id = Guid.NewGuid();

            await _service.DeleteAsync(id);

            _mockRepo.Verify(r => r.DeleteAsync(id), Times.Once);
            _mockUoW.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllClients_ShouldReturnList()
        {
            var data = new List<Cliente>()
            {
                new Cliente { Nombre = "Albert", Correo = "pujols@gmail.com" }
            };

            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(data);

            _mockMapper.Setup(m => m.Map<IEnumerable<ObtenerClienteDto>>(data))
                       .Returns(new List<ObtenerClienteDto> { new ObtenerClienteDto { Nombre = "Albert" } });

            var result = await _service.GetAllAsync();

            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.NotEmpty(result.Data!);
        }
    }
}
