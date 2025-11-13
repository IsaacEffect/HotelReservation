using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Moq;
using HotelReservation.Application.Interfaces;
using HotelReservation.Application.Services;
using HotelReservation.Domain.Entities;

namespace HotelReservation.Application.Test
{
    public class RoomServiceTests
    {
        private readonly Mock<IRoomRepository> _repoMock;
        private readonly RoomService _service;

        public RoomServiceTests()
        {
            _repoMock = new Mock<IRoomRepository>();
            _service = new RoomService(_repoMock.Object);
        }

        [Fact]
        public async Task CreateRoom_Valid_CreatesAndReturnsDto()
        {
            // arrange
            _repoMock.Setup(r => r.GetByNumberAsync(It.IsAny<int>()))
                .ReturnsAsync((Room?)null);

            _repoMock.Setup(r => r.AddAsync(It.IsAny<Room>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            // act
            var dto = await _service.CreateRoomAsync(101, Guid.NewGuid(), 50m);

            // assert
            Assert.Equal(101, dto.Number);
            Assert.Equal(50m, dto.Price);
            _repoMock.Verify(r => r.AddAsync(It.IsAny<Room>()), Times.Once);
        }

        [Fact]
        public async Task CreateRoom_DuplicateNumber_ThrowsInvalidOperationException()
        {
            // arrange
            var existing = new Room(101, Guid.NewGuid(), 60m);
            _repoMock.Setup(r => r.GetByNumberAsync(101)).ReturnsAsync(existing);

            // act & assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateRoomAsync(101, Guid.NewGuid(), 60m));
        }

        [Fact]
        public async Task GetRoom_NotFound_ReturnsNull()
        {
            // arrange
            _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Room?)null);

            // act
            var result = await _service.GetRoomAsync(Guid.NewGuid());

            // assert
            Assert.Null(result);
        }

        [Fact]
        public async Task ChangeStatus_ExistingRoom_UpdatesStatus()
        {
            // arrange
            var room = new Room(200, Guid.NewGuid(), 70m);
            var id = room.Id;
            _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(room);
            _repoMock.Setup(r => r.UpdateAsync(It.IsAny<Room>())).Returns(Task.CompletedTask).Verifiable();

            // act
            await _service.ChangeStatusAsync(id, RoomStatus.Maintenance);

            // assert
            Assert.Equal(RoomStatus.Maintenance, room.Status);
            _repoMock.Verify(r => r.UpdateAsync(It.IsAny<Room>()), Times.Once);
        }

        [Fact]
        public async Task Delete_ExistingRoom_Deletes()
        {
            // arrange
            var room = new Room(300, Guid.NewGuid(), 100m);
            _repoMock.Setup(r => r.GetByIdAsync(room.Id)).ReturnsAsync(room);
            _repoMock.Setup(r => r.DeleteAsync(room.Id)).Returns(Task.CompletedTask).Verifiable();

            // act
            await _service.DeleteAsync(room.Id);

            // assert
            _repoMock.Verify(r => r.DeleteAsync(room.Id), Times.Once);
        }
    }
}
