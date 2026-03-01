using Ticket_System_Backend.Models;

namespace Ticket_System_Backend.Repositories
{
    public interface ICommentRepository
    {
        Task<IEnumerable<Comment>> GetByTicketIdAsync(int ticketId);
        Task AddAsync(Comment comment);
    }
}