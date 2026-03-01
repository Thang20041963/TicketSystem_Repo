using System.ComponentModel.DataAnnotations;

namespace Ticket_System_Backend.DTOs
{
    public class AssignTicketRequest
    {
        [Required]
        public int AssigneeId { get; set; }
    }
}
