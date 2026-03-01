using System.Net.Http;
using System.Net.Http.Json;
using Ticket_System_App.Models;

namespace Ticket_System_App.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private static ApiService? _instance;

        public static ApiService Instance => _instance ??= new ApiService();

        // Lưu token sau khi login thành công
        public string? Token { get; set; }
        public string? Username { get; set; }
        public string? Role { get; set; }

        private ApiService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7200")
            };
        }

        public async Task<LoginResponse> LoginAsync(string username, string password)
        {
            var request = new LoginRequest
            {
                Username = username,
                Password = password
            };

            var response = await _httpClient.PostAsJsonAsync("/api/Auth/Login", request);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception(response.StatusCode == System.Net.HttpStatusCode.Unauthorized
                    ? "Sai tên đăng nhập hoặc mật khẩu"
                    : $"Lỗi: {response.StatusCode}");
            }

            var result = await response.Content.ReadFromJsonAsync<LoginResponse>()
                ?? throw new Exception("Không thể đọc response từ server");

            // Lưu thông tin đăng nhập
            Token = result.Token;
            Username = result.Username;
            Role = result.Role;

            // Thêm token vào header cho các request sau
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);

            return result;
        }
    }
}
