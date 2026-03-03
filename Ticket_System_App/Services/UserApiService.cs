using System.Net.Http.Json;
using Ticket_System_App.Models;

namespace Ticket_System_App.Services
{
    public class UserApiService
    {
        private static UserApiService? _instance;
        public static UserApiService Instance => _instance ??= new UserApiService();

        private readonly HttpClientService _http = HttpClientService.Instance;

        private UserApiService() { }

        /// <summary>Lấy danh sách supporters (ADMIN / SUPPORTER)</summary>
        public async Task<List<UserModel>> GetSupportersAsync()
        {
            var result = await _http.Client.GetFromJsonAsync<List<UserModel>>("/api/Users/supporters");
            return result ?? new List<UserModel>();
        }
    }
}
