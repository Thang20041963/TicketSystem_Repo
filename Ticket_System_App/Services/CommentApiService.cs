using System.Net.Http.Json;
using Ticket_System_App.Models;

namespace Ticket_System_App.Services
{
    public class CommentApiService
    {
        private static CommentApiService? _instance;
        public static CommentApiService Instance => _instance ??= new CommentApiService();

        private readonly HttpClientService _http = HttpClientService.Instance;

        private CommentApiService() { }

        public async Task<List<CommentModel>> GetByTicketIdAsync(int ticketId)
        {
            var result = await _http.Client.GetFromJsonAsync<List<CommentModel>>(
                $"/api/tickets/{ticketId}/comments");
            return result ?? new List<CommentModel>();
        }

        public async Task<CommentModel> AddAsync(int ticketId, string content)
        {
            var response = await _http.Client.PostAsJsonAsync(
                $"/api/tickets/{ticketId}/comments",
                new { Content = content });
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<CommentModel>()
                ?? throw new Exception("Không thể thêm comment");
        }
    }
}
