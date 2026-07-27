using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using SupportTicketManagement.Api.Data;
using SupportTicketManagement.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS for Angular frontend
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

// Database
builder.Services.AddDbContext<SupportTicketContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ITicketStatusService, TicketStatusService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

if (app.Environment.IsEnvironment("Testing"))
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<SupportTicketContext>();
    db.Database.EnsureCreated();

    if (!db.Users.Any())
    {
        db.Users.AddRange(
            new SupportTicketManagement.Api.Models.User { Id = 1, Name = "Alice Admin", Email = "alice@example.com", Role = "Admin" },
            new SupportTicketManagement.Api.Models.User { Id = 2, Name = "Bob Support", Email = "bob@example.com", Role = "Support" },
            new SupportTicketManagement.Api.Models.User { Id = 3, Name = "Charlie User", Email = "charlie@example.com", Role = "User" },
            new SupportTicketManagement.Api.Models.User { Id = 4, Name = "Diana Support", Email = "diana@example.com", Role = "Support" },
            new SupportTicketManagement.Api.Models.User { Id = 5, Name = "Ethan Analyst", Email = "ethan@example.com", Role = "User" });
        db.SaveChanges();
    }
}

app.Run();

// Expose Program class for WebApplicationFactory in tests
public partial class Program;
