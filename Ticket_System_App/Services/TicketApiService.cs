using System.Net.Http.Json;
using Ticket_System_App.Models;

namespace Ticket_System_App.Services
{
    public class TicketApiService
    {
        private static TicketApiService? _instance;
        public static TicketApiService Instance => _instance ??= new TicketApiService();

        private readonly HttpClientService _http = HttpClientService.Instance;

        private TicketApiService() { }

        /// <summary>Lấy tất cả tickets (SUPPORTER / ADMIN)</summary>
        public async Task<List<TicketModel>> GetAllAsync(
            string? status = null, string? priority = null, int? assigneeId = null)
        {
            var url = "/api/Tickets";
            var query = new List<string>();
            if (!string.IsNullOrEmpty(status)) query.Add($"status={status}");
            if (!string.IsNullOrEmpty(priority)) query.Add($"priority={priority}");
            if (assigneeId.HasValue) query.Add($"assigneeId={assigneeId}");
            if (query.Count > 0) url += "?" + string.Join("&", query);

            var result = await _http.Client.GetFromJsonAsync<List<TicketModel>>(url);
            return result ?? new List<TicketModel>();
        }

        /// <summary>Lấy tickets của mình (EMPLOYEE)</summary>
        public async Task<List<TicketModel>> GetMyTicketsAsync()
        {
            var result = await _http.Client.GetFromJsonAsync<List<TicketModel>>("/api/Tickets/my");
            return result ?? new List<TicketModel>();
        }

        /// <summary>Chi tiết ticket</summary>
        public async Task<TicketDetailModel> GetByIdAsync(int id)
        {
            var result = await _http.Client.GetFromJsonAsync<TicketDetailModel>($"/api/Tickets/{id}");
            return result ?? throw new Exception($"Ticket #{id} không tồn tại");
        }

        /// <summary>Tạo ticket mới (EMPLOYEE)</summary>
        public async Task<TicketModel> CreateAsync(CreateTicketRequestModel request)
        {
            var response = await _http.Client.PostAsJsonAsync("/api/Tickets", request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TicketModel>()
                ?? throw new Exception("Không thể tạo ticket");
        }

        /// <summary>Cập nhật trạng thái ticket</summary>
        public async Task UpdateStatusAsync(int id, string newStatus)
        {
            var response = await _http.Client.PutAsJsonAsync(
                $"/api/Tickets/{id}/status", new { NewStatus = newStatus });
            response.EnsureSuccessStatusCode();
        }

        /// <summary>Assign ticket cho supporter (ADMIN / SUPPORTER)</summary>
        public async Task AssignAsync(int id, int assigneeId)
        {
            var response = await _http.Client.PutAsJsonAsync(
                $"/api/Tickets/{id}/assign", new { AssigneeId = assigneeId });
            response.EnsureSuccessStatusCode();
        }

        /// <summary>Xoá ticket (ADMIN)</summary>
        public async Task DeleteAsync(int id)
        {
            var response = await _http.Client.DeleteAsync($"/api/Tickets/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
