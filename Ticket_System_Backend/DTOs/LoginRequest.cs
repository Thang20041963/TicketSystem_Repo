using System.ComponentModel.DataAnnotations;

namespace Ticket_System_Backend.DTOs
{
    public class LoginRequest
    {
        [Required]
        public string Username{ get; set; }
        [Required]
        public string Password { get; set; }
    }
}
