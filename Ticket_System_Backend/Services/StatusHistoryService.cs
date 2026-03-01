using Ticket_System_Backend.DTOs;
using Ticket_System_Backend.Repositories;

namespace Ticket_System_Backend.Services
{
    public class StatusHistoryService : IStatusHistoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public StatusHistoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<StatusHistoryResponse>> GetByTicketIdAsync(int ticketId)
        {
            // Validate ticket exists
            var ticket = await _unitOfWork.Tickets.GetByIdAsync(ticketId)
                ?? throw new KeyNotFoundException($"Ticket with ID {ticketId} not found.");

            var histories = await _unitOfWork.StatusHistories.GetByTicketIdAsync(ticketId);
            return histories.Select(h => new StatusHistoryResponse
            {
                Id = h.Id,
                OldStatus = h.OldStatus.ToString(),
                NewStatus = h.NewStatus.ToString(),
                ChangedByUserName = h.User?.UserName ?? string.Empty,
                ChangedAt = h.ChangedAt
            }).ToList();
        }
    }
}
