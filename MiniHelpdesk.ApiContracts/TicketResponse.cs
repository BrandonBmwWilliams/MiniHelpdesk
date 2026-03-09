namespace MiniHelpdesk.ApiContracts;

public sealed class TicketResponse
{
    public Guid Id { get; init; }
    public string CustomerDisplayName { get; init; } = "";
    public string CustomerEmail { get; init; } = "";
    public string Title { get; init; } = "";
    public string Description { get; init; } = "";
    public string Status { get; set; } = "";
    public DateTimeOffset CreatedUtc { get; init; }
    public DateTimeOffset LastUpdatedUtc { get; init; }
}
