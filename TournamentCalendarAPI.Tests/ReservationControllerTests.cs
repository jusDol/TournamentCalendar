using Microsoft.AspNetCore.Mvc;
using Moq;
using TournamentCalendarAPI.Controllers;
using TournamentCalendarAPI.Models;
using TournamentCalendarAPI.Repository;

namespace TournamentCalendarAPI.Tests
{
    public class ReservationControllerTests
    {
        private readonly Mock<IReservationRepository> _mockReservationRepository;
        private readonly Mock<IEventRepository> _mockEventRepository;
        private readonly ReservationController _controller;

        public ReservationControllerTests()
        {
            _mockReservationRepository = new Mock<IReservationRepository>();
            _mockEventRepository = new Mock<IEventRepository>();
            _controller = new ReservationController(_mockReservationRepository.Object, _mockEventRepository.Object);
        }

        [Fact]
        public async Task GetReservationsForEvent_ValidEventId_ReturnsOkResult()
        {
            // Arrange
            var eventId = 1;
            var reservations = new List<Reservation>
            {
                new Reservation { Id = 1, UserName = "Alice", Email = "alice@example.com", EventId = eventId },
                new Reservation { Id = 2, UserName = "Bob", Email = "bob@example.com", EventId = eventId }
            };

            _mockReservationRepository.Setup(repo => repo.GetAllForEventAsync(eventId))
                .ReturnsAsync(reservations);

            // Act
            var result = await _controller.GetReservationsForEvent(eventId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedReservations = Assert.IsType<List<Reservation>>(okResult.Value);
            Assert.Equal(2, returnedReservations.Count);
        }

        [Fact]
        public async Task AddReservation_ValidReservation_ReturnsOkResult()
        {
            // Arrange
            var newReservation = new Reservation { Id = 1, UserName = "Charlie", Email = "charlie@example.com", EventId = 1 };
            var relatedEvent = new Event { Id = 1, Title = "Test Event", MaxParticipants = 2 };
            var existingReservations = new List<Reservation>
            {
                new Reservation { Id = 2, UserName = "Bob", Email = "bob@example.com", EventId = 1 }
            };

            _mockEventRepository.Setup(repo => repo.GetByIdAsync(newReservation.EventId))
                .ReturnsAsync(relatedEvent);

            _mockReservationRepository.Setup(repo => repo.GetAllForEventAsync(newReservation.EventId))
                .ReturnsAsync(existingReservations);

            _mockReservationRepository.Setup(repo => repo.AddAsync(newReservation))
                .ReturnsAsync(newReservation);

            // Act
            var result = await _controller.AddReservation(newReservation);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedReservation = Assert.IsType<Reservation>(okResult.Value);
            Assert.Equal(newReservation.Id, returnedReservation.Id);
        }

        [Fact]
        public async Task AddReservation_EventNotFound_ReturnsNotFound()
        {
            // Arrange
            var newReservation = new Reservation { Id = 1, UserName = "Charlie", Email = "charlie@example.com", EventId = 99 };

            _mockEventRepository.Setup(repo => repo.GetByIdAsync(newReservation.EventId))
                .ReturnsAsync((Event)null);

            // Act
            var result = await _controller.AddReservation(newReservation);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Contains("Event with ID", notFoundResult.Value.ToString());
        }

        [Fact]
        public async Task AddReservation_EventMaxParticipantsReached_ReturnsBadRequest()
        {
            // Arrange
            var newReservation = new Reservation { Id = 3, UserName = "Derek", Email = "derek@example.com", EventId = 1 };
            var relatedEvent = new Event { Id = 1, Title = "Full Event", MaxParticipants = 2 };
            var existingReservations = new List<Reservation>
            {
                new Reservation { Id = 1, UserName = "Alice", Email = "alice@example.com", EventId = 1 },
                new Reservation { Id = 2, UserName = "Bob", Email = "bob@example.com", EventId = 1 }
            };

            _mockEventRepository.Setup(repo => repo.GetByIdAsync(newReservation.EventId))
                .ReturnsAsync(relatedEvent);

            _mockReservationRepository.Setup(repo => repo.GetAllForEventAsync(newReservation.EventId))
                .ReturnsAsync(existingReservations);

            // Act
            var result = await _controller.AddReservation(newReservation);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("has reached the maximum number of participants", badRequestResult.Value.ToString());
        }

        [Fact]
        public async Task Delete_ValidId_ReturnsNoContent()
        {
            // Arrange
            var reservationId = 1;

            _mockReservationRepository.Setup(repo => repo.DeleteAsync(reservationId))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(reservationId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_InvalidId_ReturnsNotFound()
        {
            // Arrange
            var reservationId = 99;

            _mockReservationRepository.Setup(repo => repo.DeleteAsync(reservationId))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.Delete(reservationId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}