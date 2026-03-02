using Microsoft.EntityFrameworkCore;
using Ticket_System_Backend.Models;

namespace Ticket_System_Backend.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly TicketSystemContext _context;

        public RefreshTokenRepository(TicketSystemContext context)
        {
            _context = context;
        }

        public async Task<RefreshToken?> GetByTokenAsync(string token)
            => await _context.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == token);

        public async Task AddAsync(RefreshToken refreshToken)
            => await _context.RefreshTokens.AddAsync(refreshToken);

        public async Task RevokeAllByUserIdAsync(int userId)
        {
            var activeTokens = await _context.RefreshTokens
                .Where(rt => rt.UserId == userId && rt.RevokedAt == null)
                .ToListAsync();

            foreach (var token in activeTokens)
            {
                token.RevokedAt = DateTime.UtcNow;
            }
        }
    }
}
