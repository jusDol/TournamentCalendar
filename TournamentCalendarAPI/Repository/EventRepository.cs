using TournamentCalendarAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace TournamentCalendarAPI.Repository
{
    public interface IEventRepository
    {
        Task<IEnumerable<Event>> GetAllAsync();
        Task<Event> GetByIdAsync(int id);
        Task<Event> AddAsync(Event newEvent);
        Task<Event> UpdateAsync(Event updatedEvent);
        Task<bool> DeleteAsync(int id);
    }

    public class EventRepository : IEventRepository
    {
        private readonly ApplicationDbContext _context;

        public EventRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Event>> GetAllAsync()
        {
            return await _context.Events.Include(e => e.Reservations).ToListAsync();
        }

        public async Task<Event> GetByIdAsync(int id)
        {
            return await _context.Events.Include(e => e.Reservations)
                                        .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Event> AddAsync(Event newEvent)
        {
            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync();
            return newEvent;
        }

        public async Task<Event> UpdateAsync(Event updatedEvent)
        {
            var existingEvent = await _context.Events.FindAsync(updatedEvent.Id);
            if (existingEvent == null) return null;

            existingEvent.Title = updatedEvent.Title;
            existingEvent.Start = updatedEvent.Start;
            existingEvent.End = updatedEvent.End;
            await _context.SaveChangesAsync();

            return existingEvent;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var eventEntity = await _context.Events.Include(e => e.Reservations).FirstOrDefaultAsync(e => e.Id == id);

            if (eventEntity != null)
            {
                _context.Reservations.RemoveRange(eventEntity.Reservations);
                _context.Events.Remove(eventEntity);
                await _context.SaveChangesAsync();
            }
            return true;
        }
    }
}