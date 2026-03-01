using Ticket_System_Backend.DTOs;
using Ticket_System_Backend.Models;

namespace Ticket_System_Backend.Services
{
    public interface ITicketService
    {
        Task<TicketResponse> CreateAsync(int creatorId, CreateTicketRequest request);
        Task<List<TicketResponse>> GetAllAsync(TicketStatus? status, Priority? priority, int? assigneeId);
        Task<List<TicketResponse>> GetMyTicketsAsync(int creatorId);
        Task<TicketDetailResponse> GetByIdAsync(int id);
        Task AssignAsync(int ticketId, AssignTicketRequest request);
        Task UpdateStatusAsync(int ticketId, int userId, string role, UpdateStatusRequest request);
        Task DeleteAsync(int id);
    }
}
