using Ticket_System_Backend.Models;

namespace Ticket_System_Backend.Repositories
{
    public interface IStatusHistoryRepository
    {
        Task<IEnumerable<StatusHistory>> GetByTicketIdAsync(int ticketId);
        Task AddAsync(StatusHistory history);
    }
}