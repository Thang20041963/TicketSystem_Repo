using Microsoft.EntityFrameworkCore;
using Ticket_System_Backend.Models;

namespace Ticket_System_Backend.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly TicketSystemContext _context;

        public UserRepository(TicketSystemContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByUsernameAsync(string username)
            => await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == username);

        public async Task<User?> GetByIdAsync(int id)
            => await _context.Users.FindAsync(id);

        public async Task<IEnumerable<User>> GetAllAsync()
            => await _context.Users.ToListAsync();

        public async Task AddAsync(User user)
            => await _context.Users.AddAsync(user);

        public Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
                _context.Users.Remove(user);
        }

        public async Task<IEnumerable<User>> GetByRoleAsync(Role role)
            => await _context.Users.Where(u => u.Role == role).ToListAsync();
    }
}
