using Microsoft.AspNetCore.SignalR;

namespace Ticket_System_Backend.Hubs
{
    public class TicketHub : Hub
    {
        public async Task JoinTicketAsync(int ticketId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"ticket-{ticketId}");
        }

        public async Task LeaveTicketAsync(int ticketId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"ticket-{ticketId}");
        }
    }
}
