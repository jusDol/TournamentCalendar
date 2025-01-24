using Microsoft.AspNetCore.Mvc;
using TournamentCalendarAPI.Models;
using TournamentCalendarAPI.Repository;

namespace TournamentCalendarAPI.Controllers
{
    [ApiController]
    [Route("api/reservations")]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IEventRepository _eventRepository;

        public ReservationController(IReservationRepository reservationRepository, IEventRepository eventRepository)
        {
            _reservationRepository = reservationRepository;
            _eventRepository = eventRepository;
        }

        [HttpGet("get-reservations-for-event/{eventId}")]
        public async Task<IActionResult> GetReservationsForEvent(int eventId)
        {
            var reservations = await _reservationRepository.GetAllForEventAsync(eventId);
            return Ok(reservations);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddReservation([FromBody] Reservation newReservation)
        {
            try
            {
                var relatedEvent = await _eventRepository.GetByIdAsync(newReservation.EventId);
                if (relatedEvent == null)
                {
                    return NotFound($"Event with ID {newReservation.EventId} not found.");
                }


                var createdReservation = await _reservationRepository.AddAsync(newReservation);
                return Ok(createdReservation);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("remove/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _reservationRepository.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
    

