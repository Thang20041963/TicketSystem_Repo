using System.ComponentModel.DataAnnotations;

namespace Ticket_System_Backend.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int TicketId { get; set; }
        public Ticket? Ticket { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
