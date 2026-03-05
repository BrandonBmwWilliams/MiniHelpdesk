using MiniHelpdesk.Domain;

namespace MiniHelpdesk.Application;

public sealed class TicketService : ITicketService
{
    private readonly ITicketRepository _repo;

    public TicketService(ITicketRepository repo)
    {
        _repo = repo;
    }

    public async Task<Ticket> CreateAsync(string customerDisplayName, string customerEmail, string title, string description, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var customer = new Customer(customerDisplayName, customerEmail);
        var ticket = new Ticket(customer, title, description);

        await _repo.AddAsync(ticket, ct);

        return ticket;
    }

    public Task<IReadOnlyList<Ticket>> GetAllAsync(CancellationToken ct)
    {
        return _repo.GetAllAsync(ct);
    }

    public Task<Ticket?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return _repo.GetByIdAsync(id, ct);
    }

    public async Task UpdateDetailsAsync(Guid id, string title, string description, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        Ticket? ticket = await _repo.GetByIdAsync(id, ct);
        if (ticket is null)
            throw new InvalidOperationException("Ticket not found.");

        ticket.UpdateDetails(title, description);

        await _repo.UpdateAsync(ticket, ct);
    }

    public async Task ChangeStatusAsync(Guid id, TicketStatus newStatus, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        Ticket? ticket = await _repo.GetByIdAsync(id, ct);
        if (ticket is null)
            throw new InvalidOperationException("Ticket not found.");

        ticket.MoveTo(newStatus);

        await _repo.UpdateAsync(ticket, ct);
    }
}
