using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Ticket_System_Backend.DTOs;
using Ticket_System_Backend.Models;
using Ticket_System_Backend.Services;

namespace Ticket_System_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketsController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        private int GetUserId()
            => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        private string GetUserRole()
            => User.FindFirstValue(ClaimTypes.Role)!;

        /// <summary>
        /// Create a new ticket (Employee only)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "EMPLOYEE")]
        public async Task<IActionResult> Create([FromBody] CreateTicketRequest request)
        {
            var result = await _ticketService.CreateAsync(GetUserId(), request);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Get all tickets with optional filters (Supporter/Admin only)
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "SUPPORTER,ADMIN")]
        public async Task<IActionResult> GetAll(
            [FromQuery] TicketStatus? status,
            [FromQuery] Priority? priority,
            [FromQuery] TicketCategory? category,
            [FromQuery] int? assigneeId)
        {
            var result = await _ticketService.GetAllAsync(status, priority, category, assigneeId);
            return Ok(result);
        }

        /// <summary>
        /// Get my own tickets (Employee only)
        /// </summary>
        [HttpGet("my")]
        [Authorize(Roles = "EMPLOYEE")]
        public async Task<IActionResult> GetMyTickets()
        {
            var result = await _ticketService.GetMyTicketsAsync(GetUserId());
            return Ok(result);
        }

        /// <summary>
        /// Get ticket detail by id (any authenticated user)
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _ticketService.GetByIdAsync(id);
            return Ok(result);
        }

        /// <summary>
        /// Assign ticket to a supporter (Admin/Supporter only)
        /// </summary>
        [HttpPut("{id}/assign")]
        [Authorize(Roles = "ADMIN,SUPPORTER")]
        public async Task<IActionResult> Assign(int id, [FromBody] AssignTicketRequest request)
        {
            await _ticketService.AssignAsync(id, request);
            return NoContent();
        }

        /// <summary>
        /// Update ticket status (business rules enforced in service)
        /// </summary>
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateStatusRequest request)
        {
            await _ticketService.UpdateStatusAsync(id, GetUserId(), GetUserRole(), request);
            return NoContent();
        }

        /// <summary>
        /// Delete a ticket (Admin only)
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Delete(int id)
        {
            await _ticketService.DeleteAsync(id);
            return NoContent();
        }
    }
}
