using MiniHelpdesk.ApiContracts;
using MiniHelpdesk.Ui.Components;

var builder = WebApplication.CreateBuilder(args);

// Blazor server-side rendering + interactivity
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Typed HttpClient to call the API
builder.Services.AddHttpClient<TicketsApiClient>(client =>
{
    var env = builder.Environment.EnvironmentName;
    var baseUrl = builder.Configuration["ApiBaseUrl"];

    Console.WriteLine($"UI Environment = {env}");
    Console.WriteLine($"UI Config ApiBaseUrl = {baseUrl ?? "<null>"}");

    // Force a safe fallback (API host)
    client.BaseAddress = new Uri(baseUrl ?? "http://localhost:32600/");

    Console.WriteLine($"UI HttpClient.BaseAddress = {client.BaseAddress}");
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

public sealed class TicketsApiClient
{
    private readonly HttpClient _http;

    public TicketsApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<IReadOnlyList<TicketResponse>> GetAllAsync(CancellationToken ct)
    {
        var items = await _http.GetFromJsonAsync<List<TicketResponse>>("tickets", ct) ?? new List<TicketResponse>();
        return items;
    }

    public async Task<TicketResponse?> CreateAsync(CreateTicketRequest request, CancellationToken ct)
    {
        using var resp = await _http.PostAsJsonAsync("tickets", request, ct);
        if (!resp.IsSuccessStatusCode)
            return null;

        return await resp.Content.ReadFromJsonAsync<TicketResponse>(cancellationToken: ct);
    }

    public async Task<bool> UpdateDetailsAsync(Guid id, UpdateTicketDetailsRequest request, CancellationToken ct)
    {
        using var resp = await _http.PutAsJsonAsync($"tickets/{id}/details", request, ct);
        return resp.IsSuccessStatusCode;
    }

    public async Task<bool> ChangeStatusAsync(Guid id, ChangeTicketStatusRequest request, CancellationToken ct)
    {
        using var resp = await _http.PutAsJsonAsync($"tickets/{id}/status", request, ct);
        return resp.IsSuccessStatusCode;
    }
}
