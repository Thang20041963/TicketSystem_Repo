namespace Ticket_System_Backend.Models
{
    public enum Role
    {
        EMPLOYEE = 0,
        SUPPORTER = 1,
        ADMIN = 2
    }

    public enum Priority
    {
        LOW = 0,
        MEDIUM = 1,
        HIGH = 2,
        URGENT = 3
    }

    public enum TicketStatus
    {
        OPEN = 0,
        IN_PROGRESS = 1,
        RESOLVED = 2,
        CLOSED = 3
    }
}
