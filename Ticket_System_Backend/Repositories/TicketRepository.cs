using Microsoft.EntityFrameworkCore;
using Ticket_System_Backend.Models;

namespace Ticket_System_Backend.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly TicketSystemContext _context;

        public TicketRepository(TicketSystemContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Ticket>> GetAllAsync(TicketStatus? status, Priority? priority,TicketCategory? category , int? assigneeId)
        {
            var query = _context.Tickets
                .Include(t => t.Creator)
                .Include(t => t.Assignee)
                .AsQueryable();

            if (status.HasValue)
                query = query.Where(t => t.Status == status.Value);

            if (priority.HasValue)
                query = query.Where(t => t.Priority == priority.Value);
            if (category.HasValue)
                query = query.Where(t => t.Category == category.Value);
            if (assigneeId.HasValue)
                query = query.Where(t => t.AssigneeId == assigneeId.Value);

            return await query.OrderByDescending(t => t.CreatedAt).ToListAsync();
        }

        public async Task<IEnumerable<Ticket>> GetByCreatorIdAsync(int creatorId)
        {
            return await _context.Tickets
                .Include(t => t.Creator)
                .Include(t => t.Assignee)
                .Where(t => t.CreatorId == creatorId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<Ticket?> GetByIdAsync(int id)
        {
            return await _context.Tickets
                .Include(t => t.Creator)
                .Include(t => t.Assignee)
                .Include(t => t.Comments)
                    .ThenInclude(c => c.User)
                .Include(t => t.StatusHistories)
                    .ThenInclude(h => h.User)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task AddAsync(Ticket ticket)
            => await _context.Tickets.AddAsync(ticket);

        public async Task DeleteAsync(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket != null)
                _context.Tickets.Remove(ticket);
        }

        
    }
}