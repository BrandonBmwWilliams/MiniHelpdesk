using MiniHelpdesk.Domain;

namespace MiniHelpdesk.Application;

public sealed class InMemoryTicketRepository : ITicketRepository
{
    private readonly List<Ticket> _tickets = new();

    public Task<Ticket?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        Ticket? found = _tickets.FirstOrDefault(t => t.Id == id);
        return Task.FromResult(found);
    }

    public Task<IReadOnlyList<Ticket>> GetAllAsync(CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        IReadOnlyList<Ticket> snapshot = _tickets.ToList();
        return Task.FromResult(snapshot);
    }

    public Task AddAsync(Ticket ticket, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        _tickets.Add(ticket);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Ticket ticket, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        int index = _tickets.FindIndex(t => t.Id == ticket.Id);
        if (index < 0)
            throw new InvalidOperationException("Ticket not found.");

        _tickets[index] = ticket;
        return Task.CompletedTask;
    }
}
