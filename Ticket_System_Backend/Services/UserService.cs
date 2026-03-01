using System.Security.Cryptography;
using System.Text;
using Ticket_System_Backend.DTOs;
using Ticket_System_Backend.Models;
using Ticket_System_Backend.Repositories;

namespace Ticket_System_Backend.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<UserResponse>> GetAllAsync()
        {
            var users = await _unitOfWork.Users.GetAllAsync();
            return users.Select(MapToResponse).ToList();
        }

        public async Task<UserResponse> GetByIdAsync(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"User with ID {id} not found.");

            return MapToResponse(user);
        }

        public async Task<UserResponse> CreateAsync(CreateUserRequest request)
        {
            // Check for duplicate username
            var existing = await _unitOfWork.Users.GetByUsernameAsync(request.UserName);
            if (existing != null)
                throw new InvalidOperationException($"Username '{request.UserName}' is already taken.");

            var user = new User
            {
                UserName = request.UserName,
                PasswordHash = HashPassword(request.Password),
                Email = request.Email,
                FullName = request.FullName,
                Role = request.Role,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return MapToResponse(user);
        }

        public async Task UpdateAsync(int id, UpdateUserRequest request)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"User with ID {id} not found.");

            if (request.Email != null)
                user.Email = request.Email;

            if (request.FullName != null)
                user.FullName = request.FullName;

            if (request.Role.HasValue)
                user.Role = request.Role.Value;

            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"User with ID {id} not found.");

            await _unitOfWork.Users.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            return Convert.ToBase64String(
                sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));
        }

        private static UserResponse MapToResponse(User user)
        {
            return new UserResponse
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FullName = user.FullName,
                Role = user.Role.ToString(),
                CreatedAt = user.CreatedAt
            };
        }
    }
}
