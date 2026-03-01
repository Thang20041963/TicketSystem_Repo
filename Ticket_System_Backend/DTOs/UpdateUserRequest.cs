using System.ComponentModel.DataAnnotations;
using Ticket_System_Backend.Models;

namespace Ticket_System_Backend.DTOs
{
    public class UpdateUserRequest
    {
        [EmailAddress]
        public string? Email { get; set; }

        public string? FullName { get; set; }

        public Role? Role { get; set; }
    }
}
