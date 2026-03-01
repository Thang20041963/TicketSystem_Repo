using Ticket_System_Backend.DTOs;
using Ticket_System_Backend.Models;
using Ticket_System_Backend.Repositories;

namespace Ticket_System_Backend.Services
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CommentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<CommentResponse>> GetByTicketIdAsync(int ticketId)
        {
            // Validate ticket exists
            var ticket = await _unitOfWork.Tickets.GetByIdAsync(ticketId)
                ?? throw new KeyNotFoundException($"Ticket with ID {ticketId} not found.");

            var comments = await _unitOfWork.Comments.GetByTicketIdAsync(ticketId);
            return comments.Select(c => new CommentResponse
            {
                Id = c.Id,
                Content = c.Content,
                UserName = c.User?.UserName ?? string.Empty,
                CreatedAt = c.CreatedAt
            }).ToList();
        }

        public async Task<CommentResponse> AddAsync(int ticketId, int userId, CreateCommentRequest request)
        {
            // Validate ticket exists
            var ticket = await _unitOfWork.Tickets.GetByIdAsync(ticketId)
                ?? throw new KeyNotFoundException($"Ticket with ID {ticketId} not found.");

            var comment = new Comment
            {
                Content = request.Content,
                TicketId = ticketId,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Comments.AddAsync(comment);
            await _unitOfWork.SaveChangesAsync();

            // Reload to get User navigation
            var user = await _unitOfWork.Users.GetByIdAsync(userId);

            return new CommentResponse
            {
                Id = comment.Id,
                Content = comment.Content,
                UserName = user?.UserName ?? string.Empty,
                CreatedAt = comment.CreatedAt
            };
        }
    }
}
