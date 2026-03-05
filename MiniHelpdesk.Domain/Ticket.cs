namespace MiniHelpdesk.Domain;

public sealed class Ticket
{
    public Guid Id { get; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public TicketStatus Status { get; private set; }
    public Customer Customer { get; }
    public DateTimeOffset CreatedUtc { get; }
    public DateTimeOffset LastUpdatedUtc { get; private set; }

    public Ticket(Customer customer, string title, string description)
    {
        Customer = customer ?? throw new ArgumentNullException(nameof(customer));

        Id = Guid.NewGuid();

        Title = string.IsNullOrWhiteSpace(title)
            ? throw new ArgumentException("Title is required.", nameof(title))
            : title.Trim();

        Description = string.IsNullOrWhiteSpace(description)
            ? throw new ArgumentException("Description is required.", nameof(description))
            : description.Trim();

        Status = TicketStatus.New;

        CreatedUtc = DateTimeOffset.UtcNow;
        LastUpdatedUtc = CreatedUtc;
    }

    public void UpdateDetails(string title, string description)
    {
        Title = string.IsNullOrWhiteSpace(title)
            ? throw new ArgumentException("Title is required.", nameof(title))
            : title.Trim();

        Description = string.IsNullOrWhiteSpace(description)
            ? throw new ArgumentException("Description is required.", nameof(description))
            : description.Trim();

        Touch();
    }

    public void MoveTo(TicketStatus newStatus)
    {
        if (Status == TicketStatus.Closed)
            throw new InvalidOperationException("Closed tickets cannot change status.");

        Status = newStatus;
        Touch();
    }

    private void Touch()
    {
        LastUpdatedUtc = DateTimeOffset.UtcNow;
    }
}
