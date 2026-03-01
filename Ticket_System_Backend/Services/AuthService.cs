using System.Security.Cryptography;
using System.Text;
using Ticket_System_Backend.DTOs;
using Ticket_System_Backend.Repositories;

namespace Ticket_System_Backend.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;

        public AuthService(IUserRepository userRepository, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            // 1. Tìm user theo username
            var user = await _userRepository.GetByUsernameAsync(request.Username);

            if (user == null)
                throw new UnauthorizedAccessException("Invalid username or password.");

            // 2. Verify password (hash rồi so sánh)
            if (!VerifyPassword(request.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid username or password.");

            // 3. Tạo JWT token
            var expireMinutes = 60;
            var token = _jwtService.GenerateToken(user.Id, user.UserName, user.Role);

            return new LoginResponse
            {
                Token     = token,
                Username  = user.UserName,
                Role      = user.Role.ToString(),
                ExpiresAt = DateTime.UtcNow.AddMinutes(expireMinutes)
            };
        }

        // VerifyPassword nằm ở Service — không phải Repository
        private static bool VerifyPassword(string inputPassword, string storedHash)
        {
            using var sha256 = SHA256.Create();
            var inputHash = Convert.ToBase64String(
                sha256.ComputeHash(Encoding.UTF8.GetBytes(inputPassword)));
            return inputHash == storedHash;
        }
    }
}
