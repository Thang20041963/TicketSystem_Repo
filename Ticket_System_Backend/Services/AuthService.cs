using System.Security.Cryptography;
using System.Text;
using Ticket_System_Backend.DTOs;
using Ticket_System_Backend.Models;
using Ticket_System_Backend.Repositories;

namespace Ticket_System_Backend.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public AuthService(
            IUserRepository userRepository,
            IJwtService jwtService,
            IRefreshTokenRepository refreshTokenRepository,
            IUnitOfWork unitOfWork,
            IConfiguration configuration)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _refreshTokenRepository = refreshTokenRepository;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
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

            // 3. Tạo JWT access token
            var expireMinutes = Convert.ToDouble(_configuration["Jwt:ExpireMinutes"] ?? "15");
            var token = _jwtService.GenerateToken(user.Id, user.UserName, user.Role);

            // 4. Tạo refresh token và lưu DB
            var refreshToken = await CreateRefreshTokenAsync(user.Id);

            return new LoginResponse
            {
                Token        = token,
                Username     = user.UserName,
                Role         = user.Role.ToString(),
                ExpiresAt    = DateTime.UtcNow.AddMinutes(expireMinutes),
                RefreshToken = refreshToken.Token
            };
        }

        public async Task<LoginResponse> RefreshTokenAsync(string refreshToken)
        {
            // 1. Tìm refresh token trong DB
            var storedToken = await _refreshTokenRepository.GetByTokenAsync(refreshToken);

            if (storedToken == null)
                throw new UnauthorizedAccessException("Invalid refresh token.");

            // 2. Kiểm tra token còn active không
            if (!storedToken.IsActive)
                throw new UnauthorizedAccessException("Refresh token is expired or revoked.");

            // 3. Revoke token cũ (token rotation)
            storedToken.RevokedAt = DateTime.UtcNow;

            // 4. Tạo refresh token mới
            var newRefreshToken = await CreateRefreshTokenAsync(storedToken.UserId);

            // 5. Tạo access token mới
            var user = storedToken.User;
            var expireMinutes = Convert.ToDouble(_configuration["Jwt:ExpireMinutes"] ?? "15");
            var accessToken = _jwtService.GenerateToken(user.Id, user.UserName, user.Role);

            return new LoginResponse
            {
                Token        = accessToken,
                Username     = user.UserName,
                Role         = user.Role.ToString(),
                ExpiresAt    = DateTime.UtcNow.AddMinutes(expireMinutes),
                RefreshToken = newRefreshToken.Token
            };
        }

        public async Task RevokeTokenAsync(string refreshToken)
        {
            var storedToken = await _refreshTokenRepository.GetByTokenAsync(refreshToken);

            if (storedToken == null)
                throw new UnauthorizedAccessException("Invalid refresh token.");

            if (!storedToken.IsActive)
                throw new UnauthorizedAccessException("Token is already revoked or expired.");

            storedToken.RevokedAt = DateTime.UtcNow;
            await _unitOfWork.SaveChangesAsync();
        }

        // ========== Private Helpers ==========

        private async Task<RefreshToken> CreateRefreshTokenAsync(int userId)
        {
            var refreshTokenExpireDays = Convert.ToInt32(_configuration["Jwt:RefreshTokenExpireDays"] ?? "7");

            var refreshToken = new RefreshToken
            {
                Token     = GenerateRefreshTokenString(),
                UserId    = userId,
                ExpiresAt = DateTime.UtcNow.AddDays(refreshTokenExpireDays),
                CreatedAt = DateTime.UtcNow
            };

            await _refreshTokenRepository.AddAsync(refreshToken);
            await _unitOfWork.SaveChangesAsync();

            return refreshToken;
        }

        private static string GenerateRefreshTokenString()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
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
