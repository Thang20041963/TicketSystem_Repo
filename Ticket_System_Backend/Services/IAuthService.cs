using Ticket_System_Backend.DTOs;

namespace Ticket_System_Backend.Services
{
    public interface IAuthService
    {
        Task<LoginResponse> LoginAsync(LoginRequest request);
    }
}
