using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TournamentCalendarAPI.Controllers;
using TournamentCalendarAPI.Models;
using TournamentCalendarAPI.Repository;
using Xunit;

namespace TournamentCalendarAPI.Tests
{
    public class EventControllerTests
    {
        private readonly Mock<IEventRepository> _mockEventRepository;
        private readonly EventController _controller;

        public EventControllerTests()
        {
            _mockEventRepository = new Mock<IEventRepository>();
            _controller = new EventController(_mockEventRepository.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WithListOfEvents()
        {
            // Arrange
            var events = new List<Event>
            {
                new Event { Id = 1, Title = "Pokemon Tournament", MaxParticipants = 16 },
                new Event { Id = 2, Title = "KeyForge Championship", MaxParticipants = 32 }
            };

            _mockEventRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(events);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedEvents = Assert.IsType<List<Event>>(okResult.Value);
            Assert.Equal(2, returnedEvents.Count);
        }

        [Fact]
        public async Task GetById_ValidId_ReturnsOkResult_WithEvent()
        {
            // Arrange
            var testEvent = new Event { Id = 1, Title = "Pokemon Tournament", MaxParticipants = 16 };

            _mockEventRepository.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(testEvent);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedEvent = Assert.IsType<Event>(okResult.Value);
            Assert.Equal(testEvent.Id, returnedEvent.Id);
        }

        [Fact]
        public async Task GetById_InvalidId_ReturnsNotFound()
        {
            // Arrange
            _mockEventRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Event)null);

            // Act
            var result = await _controller.GetById(99);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Add_ValidEvent_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var newEvent = new Event { Id = 3, Title = "Lorcana Event", MaxParticipants = 20 };

            _mockEventRepository.Setup(repo => repo.AddAsync(newEvent))
                .ReturnsAsync(newEvent);

            // Act
            var result = await _controller.Create(newEvent);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnedEvent = Assert.IsType<Event>(createdResult.Value);
            Assert.Equal(newEvent.Id, returnedEvent.Id);
        }

        [Fact]
        public async Task Update_ValidEvent_ReturnsOkResult()
        {
            // Arrange
            var updatedEvent = new Event { Id = 1, Title = "Updated Tournament", MaxParticipants = 25 };

            _mockEventRepository.Setup(repo => repo.UpdateAsync(updatedEvent))
                .ReturnsAsync(updatedEvent);

            // Act
            var result = await _controller.Update(1, updatedEvent);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedEvent = Assert.IsType<Event>(okResult.Value);
            Assert.Equal(updatedEvent.Title, returnedEvent.Title);
        }

        [Fact]
        public async Task Update_InvalidEventId_ReturnsBadRequest()
        {
            // Arrange
            var updatedEvent = new Event { Id = 2, Title = "Invalid Tournament", MaxParticipants = 20 };

            // Act
            var result = await _controller.Update(1, updatedEvent);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task Delete_ValidId_ReturnsNoContent()
        {
            // Arrange
            _mockEventRepository.Setup(repo => repo.DeleteAsync(1))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_InvalidId_ReturnsNotFound()
        {
            // Arrange
            _mockEventRepository.Setup(repo => repo.DeleteAsync(99))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.Delete(99);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
