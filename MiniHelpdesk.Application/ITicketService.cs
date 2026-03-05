using MiniHelpdesk.Domain;

namespace MiniHelpdesk.Application;

public interface ITicketService
{
    Task<Ticket> CreateAsync(string customerDisplayName, string customerEmail, string title, string description, CancellationToken ct);
    Task<IReadOnlyList<Ticket>> GetAllAsync(CancellationToken ct);
    Task<Ticket?> GetByIdAsync(Guid id, CancellationToken ct);
    Task UpdateDetailsAsync(Guid id, string title, string description, CancellationToken ct);
    Task ChangeStatusAsync(Guid id, TicketStatus newStatus, CancellationToken ct);
}
