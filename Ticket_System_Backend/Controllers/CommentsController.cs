using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Ticket_System_Backend.DTOs;
using Ticket_System_Backend.Services;

namespace Ticket_System_Backend.Controllers
{
    [Route("api/tickets/{ticketId}/comments")]
    [ApiController]
    [Authorize]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
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
        /// Add a comment to a ticket
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AddComment(int ticketId, [FromBody] CreateCommentRequest request)
        {
            var result = await _commentService.AddAsync(ticketId, GetUserId(), request);
            return CreatedAtAction(nameof(GetComments), new { ticketId }, result);
        }
    }
}
