using MiniHelpdesk.ApiContracts;
using MiniHelpdesk.Application;
using MiniHelpdesk.Domain;

var builder = WebApplication.CreateBuilder(args);

// Controllers (classic enterprise HTTP layer)
builder.Services.AddControllers();

// DI registrations (DIP swap points)
builder.Services.AddSingleton<ITicketRepository, InMemoryTicketRepository>();
builder.Services.AddScoped<ITicketService, TicketService>();

// Dev convenience
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
