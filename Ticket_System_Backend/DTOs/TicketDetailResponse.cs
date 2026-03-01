namespace Ticket_System_Backend.DTOs
{
    public class TicketDetailResponse
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;

        public int CreatorId { get; set; }
        public string CreatorName { get; set; } = string.Empty;

        public int? AssigneeId { get; set; }
        public string? AssigneeName { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public List<CommentResponse> Comments { get; set; } = new();
        public List<StatusHistoryResponse> StatusHistories { get; set; } = new();
    }
}
