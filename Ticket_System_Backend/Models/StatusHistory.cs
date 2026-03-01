using System.ComponentModel.DataAnnotations;

namespace Ticket_System_Backend.Models
{
    public class StatusHistory
    {
        [Key]
        public int Id { get; set; }

        public int? TicketId { get; set; }
        public Ticket? Ticket { get; set; }

        public int? UserId { get; set; }
        public User? User { get; set; }

        public TicketStatus OldStatus { get; set; }
        public TicketStatus NewStatus { get; set; }

        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
    }
}
