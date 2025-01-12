namespace TournamentCalendarAPI.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public int EventId { get; set; }
    }
}
