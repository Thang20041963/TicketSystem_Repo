using Ticket_System_Backend.Models;

namespace Ticket_System_Backend.Services
{
    public interface IJwtService
    {
        string GenerateToken(int userId, string username, Role role);
    }
}
