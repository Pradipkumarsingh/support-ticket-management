using SupportTicketManagement.Api.Models;

namespace SupportTicketManagement.Api.Services;

public interface ITicketStatusService
{
    bool IsValidTransition(TicketStatus from, TicketStatus to);
}

public class TicketStatusService : ITicketStatusService
{
    private static readonly Dictionary<TicketStatus, TicketStatus[]> AllowedTransitions = new()
    {
        { TicketStatus.Open, [TicketStatus.InProgress, TicketStatus.Cancelled] },
        { TicketStatus.InProgress, [TicketStatus.Resolved, TicketStatus.Cancelled] },
        { TicketStatus.Resolved, [TicketStatus.Closed] },
        { TicketStatus.Closed, Array.Empty<TicketStatus>() },
        { TicketStatus.Cancelled, Array.Empty<TicketStatus>() }
    };

    public bool IsValidTransition(TicketStatus from, TicketStatus to)
    {
        return AllowedTransitions.TryGetValue(from, out var allowed) && allowed.Contains(to);
    }
}

