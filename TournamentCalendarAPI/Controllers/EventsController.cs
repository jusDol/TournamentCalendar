using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TournamentCalendarAPI.Models;
using TournamentCalendarAPI.Repository;

namespace TournamentCalendarAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventController : ControllerBase
    {
        private readonly IEventRepository _eventRepository;

        public EventController(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var events = await _eventRepository.GetAllAsync();
            return Ok(events);
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var eventEntity = await _eventRepository.GetByIdAsync(id);
            if (eventEntity == null) return NotFound();
            return Ok(eventEntity);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Create(Event newEvent)
        {
            var createdEvent = await _eventRepository.AddAsync(newEvent);
            return CreatedAtAction(nameof(GetById), new { id = createdEvent.Id }, createdEvent);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, Event updatedEvent)
        {
            if (id != updatedEvent.Id) return BadRequest();

            var eventEntity = await _eventRepository.UpdateAsync(updatedEvent);
            if (eventEntity == null) return NotFound();

            return Ok(eventEntity);
        }

        [HttpDelete("remove/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _eventRepository.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
