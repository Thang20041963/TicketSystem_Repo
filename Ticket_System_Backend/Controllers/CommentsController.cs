using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using Ticket_System_Backend.DTOs;
using Ticket_System_Backend.Hubs;
using Ticket_System_Backend.Services;

namespace Ticket_System_Backend.Controllers
{
    [Route("api/tickets/{ticketId}/comments")]
    [ApiController]
    [Authorize]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly IHubContext<TicketHub> _hubContext;

        public CommentsController(ICommentService commentService, IHubContext<TicketHub> hubContext)
        {
            _commentService = commentService;
            _hubContext = hubContext;
        }

        private int GetUserId()
            => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        /// <summary>
        /// Get comments for a ticket
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetComments(int ticketId)
        {
            var result = await _commentService.GetByTicketIdAsync(ticketId);
            return Ok(result);
        }

        /// <summary>
        /// Add a comment to a ticket and broadcast via SignalR
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AddComment(int ticketId, [FromBody] CreateCommentRequest request)
        {
            var result = await _commentService.AddAsync(ticketId, GetUserId(), request);

            // Broadcast to all clients watching this ticket
            await _hubContext.Clients.Group($"ticket-{ticketId}")
                .SendAsync("NewComment", result);

            return CreatedAtAction(nameof(GetComments), new { ticketId }, result);
        }
    }
}
