using TournamentCalendarAPI.Models;

namespace TournamentCalendarAPI.Repository
{
    public interface IUserRepository
    {
        Task<UserDto> GetUser(string username, string password);
    }

    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserDto> GetUser(string username, string password)
        {

            var user =   _context.User.FirstOrDefault(u => u.Username == username && u.Password == password);

            return user;
        }

    }
}
