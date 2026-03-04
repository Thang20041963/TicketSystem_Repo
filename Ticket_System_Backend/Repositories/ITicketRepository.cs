using Ticket_System_Backend.Models;

namespace Ticket_System_Backend.Repositories
{
    public interface ITicketRepository
    {
        Task<IEnumerable<Ticket>> GetAllAsync(TicketStatus? status, Priority? priority, TicketCategory? category , int? assigneeId);
        Task<IEnumerable<Ticket>> GetByCreatorIdAsync(int creatorId);
        Task<Ticket?> GetByIdAsync(int id);
        Task AddAsync(Ticket ticket);
        Task DeleteAsync(int id);
    }
}