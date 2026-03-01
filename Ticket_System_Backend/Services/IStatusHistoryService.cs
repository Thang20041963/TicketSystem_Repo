using Ticket_System_Backend.DTOs;

namespace Ticket_System_Backend.Services
{
    public interface IStatusHistoryService
    {
        Task<List<StatusHistoryResponse>> GetByTicketIdAsync(int ticketId);
    }
}
