using Microsoft.EntityFrameworkCore;
using TournamentCalendarAPI.Models;

namespace TournamentCalendarAPI.Repository
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    }
}
