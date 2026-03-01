using System.ComponentModel.DataAnnotations;

namespace Ticket_System_Backend.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        public string? FullName { get; set; }

        public Role Role { get; set; } = Role.EMPLOYEE;

       
        public ICollection<Ticket> CreatedTickets { get; set; } = new List<Ticket>();
        public ICollection<Ticket> AssignedTickets { get; set; } = new List<Ticket>();

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
