namespace Ticket_System_Backend.DTOs
{
    public class TicketResponse
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
        public string CreatorName { get; set; } = string.Empty;
        public string? AssigneeName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
