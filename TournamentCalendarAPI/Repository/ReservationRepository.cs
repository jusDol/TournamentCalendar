using Microsoft.EntityFrameworkCore;
using System;
using TournamentCalendarAPI.Models;

namespace TournamentCalendarAPI.Repository
{
     public interface IReservationRepository
     {
        Task<IEnumerable<Reservation>> GetAllForEventAsync(int eventId);
        Task<Reservation> AddAsync(Reservation newReservation);
        Task<bool> DeleteAsync(int id);
     }

    public class ReservationRepository : IReservationRepository
    {
        private readonly ApplicationDbContext _context;

        public ReservationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Reservation>> GetAllForEventAsync(int eventId)
        {
            return await _context.Reservations
                                 .Where(r => r.EventId == eventId)
                                 .ToListAsync();
        }

        public async Task<Reservation> AddAsync(Reservation newReservation)
        {
            var relatedEvent = await _context.Events.FindAsync(newReservation.EventId);
            if (relatedEvent == null)
            {
                throw new KeyNotFoundException($"Event with ID {newReservation.EventId} not found.");
            }

            _context.Reservations.Add(newReservation);
            await _context.SaveChangesAsync();
            return newReservation;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existingReservation = await _context.Reservations.FindAsync(id);
            if (existingReservation == null) return false;

            _context.Reservations.Remove(existingReservation);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
