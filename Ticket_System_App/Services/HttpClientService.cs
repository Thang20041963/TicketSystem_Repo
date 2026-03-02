using System.Net.Http;

namespace Ticket_System_App.Services
{
    /// <summary>
    /// Singleton – quản lý HttpClient dùng chung + thông tin đăng nhập hiện tại
    /// </summary>
    public class HttpClientService
    {
        private static HttpClientService? _instance;
        public static HttpClientService Instance => _instance ??= new HttpClientService();

        public readonly HttpClient Client;

        // Thông tin user sau khi login
        public string? Token { get; private set; }
        public string? Username { get; private set; }
        public string? Role { get; private set; }
        public int UserId { get; private set; }

        private HttpClientService()
        {
            Client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7200")
            };
        }

        public void SetSession(string token, string username, string role, int userId)
        {
            Token = token;
            Username = username;
            Role = role;
            UserId = userId;
            Client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        public void Logout()
        {
            Token = null;
            Username = null;
            Role = null;
            UserId = 0;
            Client.DefaultRequestHeaders.Authorization = null;
        }

        public bool IsLoggedIn => !string.IsNullOrEmpty(Token);
    }
}
