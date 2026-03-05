using MiniHelpdesk.Domain;

namespace MiniHelpdesk.Application;

public interface ITicketRepository
{
    Task<Ticket?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<IReadOnlyList<Ticket>> GetAllAsync(CancellationToken ct);
    Task AddAsync(Ticket ticket, CancellationToken ct);
    Task UpdateAsync(Ticket ticket, CancellationToken ct);
}
