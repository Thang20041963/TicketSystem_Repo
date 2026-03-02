using Ticket_System_Backend.Models;

namespace Ticket_System_Backend.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> GetByTokenAsync(string token);
        Task AddAsync(RefreshToken refreshToken);
        Task RevokeAllByUserIdAsync(int userId);
    }
}
