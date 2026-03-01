using Microsoft.EntityFrameworkCore.Storage;
using Ticket_System_Backend.Models;

namespace Ticket_System_Backend.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TicketSystemContext _context;

        private IDbContextTransaction? _transaction;
        public IUserRepository Users { get; }
        public ITicketRepository Tickets { get; }
        public ICommentRepository Comments { get; }

        public IStatusHistoryRepository StatusHistories { get; }
        public UnitOfWork(TicketSystemContext context,
                          IUserRepository users,
                          ITicketRepository tickets,
                          ICommentRepository comments,
                          IStatusHistoryRepository statusHistory)
        {
            _context = context;
            Users = users;
            Tickets = tickets;
            Comments = comments;
            StatusHistories = statusHistory;
        }
        public async Task BeginTransactionAsync()
            => _transaction = await _context.Database.BeginTransactionAsync();
        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
            await _transaction!.CommitAsync();
        }
        public async Task RollbackAsync()
            => await _transaction!.RollbackAsync();
        public async Task<int> SaveChangesAsync()
            => await _context.SaveChangesAsync();
        public void Dispose()
            => _transaction?.Dispose();
    }
}
