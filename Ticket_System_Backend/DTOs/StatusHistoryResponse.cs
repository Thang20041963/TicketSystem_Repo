namespace Ticket_System_Backend.DTOs
{
    public class StatusHistoryResponse
    {
        public int Id { get; set; }
        public string OldStatus { get; set; } = string.Empty;
        public string NewStatus { get; set; } = string.Empty;
        public string ChangedByUserName { get; set; } = string.Empty;
        public DateTime ChangedAt { get; set; }
    }
}
