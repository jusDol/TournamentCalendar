using System.ComponentModel.DataAnnotations.Schema;

namespace TournamentCalendarAPI.Models
{
    [Table("users")]
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
