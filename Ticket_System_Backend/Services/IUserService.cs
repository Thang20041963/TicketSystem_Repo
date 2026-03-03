using Ticket_System_Backend.DTOs;

namespace Ticket_System_Backend.Services
{
    public interface IUserService
    {
        Task<List<UserResponse>> GetAllAsync();
        Task<UserResponse> GetByIdAsync(int id);
        Task<UserResponse> CreateAsync(CreateUserRequest request);
        Task UpdateAsync(int id, UpdateUserRequest request);
        Task DeleteAsync(int id);
        Task<List<UserResponse>> GetSupportersAsync();
    }
}
