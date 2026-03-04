using System.ComponentModel.DataAnnotations;

namespace Ticket_System_Backend.Models
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }

        [Required]
        
        public string Title { get; set; } = string.Empty;
        
        public string? Description { get; set; }

        public Priority Priority { get; set; } = Priority.MEDIUM;

        public TicketStatus Status { get; set; } = TicketStatus.OPEN;

        public int CreatorId { get; set; }
        public User? Creator { get; set; }

        public int? AssigneeId { get; set; }
        public User? Assignee { get; set; }

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        public ICollection<StatusHistory> StatusHistories { get; set; } = new List<StatusHistory>();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

         public TicketCategory Category { get; set; } = TicketCategory.GENERAL;
         public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
    }


}
