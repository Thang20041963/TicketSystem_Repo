using System.ComponentModel.DataAnnotations;
using Ticket_System_Backend.Models;

namespace Ticket_System_Backend.DTOs
{
    public class CreateTicketRequest
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public Priority Priority { get; set; } = Priority.MEDIUM;
    }
}
