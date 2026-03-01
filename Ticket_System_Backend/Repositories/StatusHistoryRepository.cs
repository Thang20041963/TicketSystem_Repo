using Microsoft.EntityFrameworkCore;
using Ticket_System_Backend.Models;

namespace Ticket_System_Backend.Repositories
{
    public class StatusHistoryRepository : IStatusHistoryRepository
    {
        private readonly TicketSystemContext _context;

        public StatusHistoryRepository(TicketSystemContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<StatusHistory>> GetByTicketIdAsync(int ticketId)
        {
            return await _context.StatusHistories
                .Include(h => h.User)
                .Where(h => h.TicketId == ticketId)
                .OrderBy(h => h.ChangedAt)
                .ToListAsync();
        }

        public async Task AddAsync(StatusHistory history)
            => await _context.StatusHistories.AddAsync(history);
    }
}