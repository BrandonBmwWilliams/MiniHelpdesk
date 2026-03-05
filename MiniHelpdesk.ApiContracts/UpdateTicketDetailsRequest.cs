namespace MiniHelpdesk.ApiContracts;

public sealed class UpdateTicketDetailsRequest
{
    public string Title { get; init; } = "";
    public string Description { get; init; } = "";
}
