using Microsoft.EntityFrameworkCore;
using Ticket_System_Backend.Models;

namespace Ticket_System_Backend.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly TicketSystemContext _context;

        public CommentRepository(TicketSystemContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Comment>> GetByTicketIdAsync(int ticketId)
        {
            return await _context.Comments
                .Include(c => c.User)
                .Where(c => c.TicketId == ticketId)
                .OrderBy(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task AddAsync(Comment comment)
            => await _context.Comments.AddAsync(comment);
    }
}