namespace MiniHelpdesk.ApiContracts;

public sealed class CreateTicketRequest
{
    public string CustomerDisplayName { get; init; } = "";
    public string CustomerEmail { get; init; } = "";
    public string Title { get; init; } = "";
    public string Description { get; init; } = "";
}
