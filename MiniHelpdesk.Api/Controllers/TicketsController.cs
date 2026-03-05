using Microsoft.AspNetCore.Mvc;
using MiniHelpdesk.ApiContracts;
using MiniHelpdesk.Application;
using MiniHelpdesk.Domain;

namespace MiniHelpdesk.Api.Controllers;

[ApiController]
[Route("tickets")]
public sealed class TicketsController : ControllerBase
{
    private readonly ITicketService _service;

    public TicketsController(ITicketService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<TicketResponse>>> GetAll(CancellationToken ct)
    {
        IReadOnlyList<Ticket> tickets = await _service.GetAllAsync(ct);
        var responses = tickets.Select(MapToResponse).ToList();
        return Ok(responses);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TicketResponse>> GetById(Guid id, CancellationToken ct)
    {
        Ticket? ticket = await _service.GetByIdAsync(id, ct);
        if (ticket is null)
            return NotFound();

        return Ok(MapToResponse(ticket));
    }

    [HttpPost]
    public async Task<ActionResult<TicketResponse>> Create([FromBody] CreateTicketRequest request, CancellationToken ct)
    {
        Ticket created = await _service.CreateAsync(
            request.CustomerDisplayName,
            request.CustomerEmail,
            request.Title,
            request.Description,
            ct);

        TicketResponse response = MapToResponse(created);
        return Created($"/tickets/{response.Id}", response);
    }

    [HttpPut("{id:guid}/details")]
    public async Task<IActionResult> UpdateDetails(Guid id, [FromBody] UpdateTicketDetailsRequest request, CancellationToken ct)
    {
        await _service.UpdateDetailsAsync(id, request.Title, request.Description, ct);
        return NoContent();
    }

    [HttpPut("{id:guid}/status")]
    public async Task<IActionResult> ChangeStatus(Guid id, [FromBody] ChangeTicketStatusRequest request, CancellationToken ct)
    {
        if (!Enum.TryParse<TicketStatus>(request.Status, ignoreCase: true, out var parsed))
            return BadRequest(new { message = "Invalid status. Use New, InProgress, Resolved, or Closed." });

        await _service.ChangeStatusAsync(id, parsed, ct);
        return NoContent();
    }

    private static TicketResponse MapToResponse(Ticket t) =>
        new()
        {
            Id = t.Id,
            CustomerDisplayName = t.Customer.DisplayName,
            CustomerEmail = t.Customer.Email,
            Title = t.Title,
            Description = t.Description,
            Status = t.Status.ToString(),
            CreatedUtc = t.CreatedUtc,
            LastUpdatedUtc = t.LastUpdatedUtc
        };
}
