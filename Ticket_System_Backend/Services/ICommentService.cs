using Ticket_System_Backend.DTOs;

namespace Ticket_System_Backend.Services
{
    public interface ICommentService
    {
        Task<List<CommentResponse>> GetByTicketIdAsync(int ticketId);
        Task<CommentResponse> AddAsync(int ticketId, int userId, CreateCommentRequest request);
    }
}
