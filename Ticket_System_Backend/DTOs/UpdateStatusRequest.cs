using System.ComponentModel.DataAnnotations;
using Ticket_System_Backend.Models;

namespace Ticket_System_Backend.DTOs
{
    public class UpdateStatusRequest
    {
        [Required]
        public TicketStatus NewStatus { get; set; }
    }
}
