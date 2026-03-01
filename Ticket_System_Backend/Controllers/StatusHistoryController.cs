using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ticket_System_Backend.Services;

namespace Ticket_System_Backend.Controllers
{
    [Route("api/tickets/{ticketId}/history")]
    [ApiController]
    [Authorize]
    public class StatusHistoryController : ControllerBase
    {
        private readonly IStatusHistoryService _statusHistoryService;

        public StatusHistoryController(IStatusHistoryService statusHistoryService)
        {
            _statusHistoryService = statusHistoryService;
        }

        /// <summary>
        /// Get status change history for a ticket
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetHistory(int ticketId)
        {
            var result = await _statusHistoryService.GetByTicketIdAsync(ticketId);
            return Ok(result);
        }
    }
}
