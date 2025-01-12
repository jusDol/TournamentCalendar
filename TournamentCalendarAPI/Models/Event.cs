namespace TournamentCalendarAPI.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public int MaxParticipants { get; set; }
        public ICollection<Reservation> Reservations { get; set; }
    }
}
