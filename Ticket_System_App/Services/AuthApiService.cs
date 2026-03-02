using System.Net.Http.Json;
using Ticket_System_App.Models;

namespace Ticket_System_App.Services
{
    public class AuthApiService
    {
        private static AuthApiService? _instance;
        public static AuthApiService Instance => _instance ??= new AuthApiService();

        private readonly HttpClientService _http = HttpClientService.Instance;

        private AuthApiService() { }

        public async Task LoginAsync(string username, string password)
        {
            var request = new LoginRequest { Username = username, Password = password };

            var response = await _http.Client.PostAsJsonAsync("/api/Auth/Login", request);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.StatusCode == System.Net.HttpStatusCode.Unauthorized
                    ? "Sai tên đăng nhập hoặc mật khẩu"
                    : $"Lỗi: {response.StatusCode}");
            }

            var result = await response.Content.ReadFromJsonAsync<LoginResponse>()
                ?? throw new Exception("Không thể đọc response từ server");

            _http.SetSession(result.Token, result.Username, result.Role, result.UserId);
        }
    }
}
