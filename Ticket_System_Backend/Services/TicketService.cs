using Ticket_System_Backend.DTOs;
using Ticket_System_Backend.Models;
using Ticket_System_Backend.Repositories;

namespace Ticket_System_Backend.Services
{
    public class TicketService : ITicketService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TicketService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TicketResponse> CreateAsync(int creatorId, CreateTicketRequest request)
        {
            var ticket = new Ticket
            {
                Title = request.Title,
                Description = request.Description,
                Priority = request.Priority,
                Status = TicketStatus.OPEN,
                CreatorId = creatorId,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Tickets.AddAsync(ticket);
            await _unitOfWork.SaveChangesAsync();

            // Reload with navigation properties
            var saved = await _unitOfWork.Tickets.GetByIdAsync(ticket.Id);
            return MapToResponse(saved!);
        }

        public async Task<List<TicketResponse>> GetAllAsync(TicketStatus? status, Priority? priority, TicketCategory? category, int? assigneeId)
        {
            var tickets = await _unitOfWork.Tickets.GetAllAsync(status, priority, category, assigneeId);
            return tickets.Select(MapToResponse).ToList();
        }

        public async Task<List<TicketResponse>> GetMyTicketsAsync(int creatorId)
        {
            var tickets = await _unitOfWork.Tickets.GetByCreatorIdAsync(creatorId);
            return tickets.Select(MapToResponse).ToList();
        }

        public async Task<TicketDetailResponse> GetByIdAsync(int id)
        {
            var ticket = await _unitOfWork.Tickets.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Ticket with ID {id} not found.");

            return new TicketDetailResponse
            {
                Id = ticket.Id,
                Title = ticket.Title,
                Description = ticket.Description,
                Status = ticket.Status.ToString(),
                Priority = ticket.Priority.ToString(),
                CreatorId = ticket.CreatorId,
                CreatorName = ticket.Creator?.UserName ?? string.Empty,
                AssigneeId = ticket.AssigneeId,
                AssigneeName = ticket.Assignee?.UserName,
                CreatedAt = ticket.CreatedAt,
                UpdatedAt = ticket.UpdatedAt,
                Comments = ticket.Comments
                    .OrderBy(c => c.CreatedAt)
                    .Select(c => new CommentResponse
                    {
                        Id = c.Id,
                        Content = c.Content,
                        UserName = c.User?.UserName ?? string.Empty,
                        CreatedAt = c.CreatedAt
                    }).ToList(),
                StatusHistories = ticket.StatusHistories
                    .OrderBy(h => h.ChangedAt)
                    .Select(h => new StatusHistoryResponse
                    {
                        Id = h.Id,
                        OldStatus = h.OldStatus.ToString(),
                        NewStatus = h.NewStatus.ToString(),
                        ChangedByUserName = h.User?.UserName ?? string.Empty,
                        ChangedAt = h.ChangedAt
                    }).ToList()
            };
        }

        public async Task AssignAsync(int ticketId, AssignTicketRequest request)
        {
            var ticket = await _unitOfWork.Tickets.GetByIdAsync(ticketId)
                ?? throw new KeyNotFoundException($"Ticket with ID {ticketId} not found.");

            // Validate assignee is a SUPPORTER
            var assignee = await _unitOfWork.Users.GetByIdAsync(request.AssigneeId)
                ?? throw new KeyNotFoundException($"User with ID {request.AssigneeId} not found.");

            if (assignee.Role != Role.SUPPORTER)
                throw new InvalidOperationException("Ticket can only be assigned to a SUPPORTER.");

            ticket.AssigneeId = request.AssigneeId;
            ticket.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateStatusAsync(int ticketId, int userId, string role, UpdateStatusRequest request)
        {
            var ticket = await _unitOfWork.Tickets.GetByIdAsync(ticketId)
                ?? throw new KeyNotFoundException($"Ticket with ID {ticketId} not found.");

            var oldStatus = ticket.Status;
            var newStatus = request.NewStatus;

            // Business rules
            if (role == Role.EMPLOYEE.ToString())
            {
                // Employee chỉ được RESOLVED → CLOSED
                if (oldStatus != TicketStatus.RESOLVED || newStatus != TicketStatus.CLOSED)
                    throw new InvalidOperationException("Employee can only change status from RESOLVED to CLOSED.");

                // Employee chỉ được close ticket của chính mình
                if (ticket.CreatorId != userId)
                    throw new UnauthorizedAccessException("You can only close your own tickets.");
            }
            else if (role == Role.SUPPORTER.ToString())
            {
                // Supporter: OPEN → IN_PROGRESS → RESOLVED
                var validTransitions = new Dictionary<TicketStatus, TicketStatus>
                {
                    { TicketStatus.OPEN, TicketStatus.IN_PROGRESS },
                    { TicketStatus.IN_PROGRESS, TicketStatus.RESOLVED }
                };

                if (!validTransitions.TryGetValue(oldStatus, out var expected) || expected != newStatus)
                    throw new InvalidOperationException(
                        $"Supporter cannot change status from {oldStatus} to {newStatus}.");
            }
            // ADMIN can do any status change

            ticket.Status = newStatus;
            ticket.UpdatedAt = DateTime.UtcNow;

            // Log status history
            var history = new StatusHistory
            {
                TicketId = ticketId,
                UserId = userId,
                OldStatus = oldStatus,
                NewStatus = newStatus,
                ChangedAt = DateTime.UtcNow
            };

            await _unitOfWork.StatusHistories.AddAsync(history);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var ticket = await _unitOfWork.Tickets.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Ticket with ID {id} not found.");

            await _unitOfWork.Tickets.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        private static TicketResponse MapToResponse(Ticket ticket)
        {
            return new TicketResponse
            {
                Id = ticket.Id,
                Title = ticket.Title,
                Status = ticket.Status.ToString(),
                Priority = ticket.Priority.ToString(),
                CreatorName = ticket.Creator?.UserName ?? string.Empty,
                AssigneeName = ticket.Assignee?.UserName,
                CreatedAt = ticket.CreatedAt
            };
        }
    }
}
