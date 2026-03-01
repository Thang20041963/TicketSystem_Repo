using System.ComponentModel.DataAnnotations;

namespace Ticket_System_Backend.DTOs
{
    public class CreateCommentRequest
    {
        [Required]
        public string Content { get; set; } = string.Empty;
    }
}
