using System.Net;
using System.Net.Http.Json;

namespace SupportTicketManagement.Tests;

public class TicketStatusIntegrationTests : IClassFixture<TestWebApplicationFactory>
{
    private readonly HttpClient _client;

    public TicketStatusIntegrationTests(TestWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    private async Task<int> CreateTicketAsync()
    {
        var response = await _client.PostAsJsonAsync("/api/tickets", new
        {
            title = "Test ticket",
            description = "Test description",
            priority = "Medium",
            createdByUserId = 3,
            assignedToUserId = 2
        });

        response.EnsureSuccessStatusCode();

        var created = await response.Content.ReadFromJsonAsync<CreateTicketResponse>();
        return created!.Id;
    }

    private sealed record CreateTicketResponse(int Id);

    private static readonly IReadOnlyDictionary<string, string[]> PathsToStatus =
        new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase)
        {
            ["Open"] = Array.Empty<string>(),
            ["InProgress"] = ["InProgress"],
            ["Resolved"] = ["InProgress", "Resolved"],
            ["Closed"] = ["InProgress", "Resolved", "Closed"],
            ["Cancelled"] = ["Cancelled"]
        };

    private async Task ReachStatusAsync(int ticketId, string status)
    {
        if (!PathsToStatus.TryGetValue(status, out var steps))
        {
            throw new ArgumentException($"Unknown status: {status}", nameof(status));
        }

        foreach (var step in steps)
        {
            var response = await _client.PostAsJsonAsync(
                $"/api/tickets/{ticketId}/status",
                new { newStatus = step });
            response.EnsureSuccessStatusCode();
        }
    }

    [Theory]
    [InlineData("Open", "InProgress")]
    [InlineData("InProgress", "Resolved")]
    [InlineData("Resolved", "Closed")]
    [InlineData("Open", "Cancelled")]
    [InlineData("InProgress", "Cancelled")]
    public async Task ValidTransitions_ShouldSucceed(string fromStatus, string toStatus)
    {
        var id = await CreateTicketAsync();
        await ReachStatusAsync(id, fromStatus);

        var response = await _client.PostAsJsonAsync($"/api/tickets/{id}/status", new { newStatus = toStatus });

        Assert.True(response.IsSuccessStatusCode);
    }

    [Theory]
    [InlineData("Open", "Resolved")]
    [InlineData("Open", "Closed")]
    [InlineData("Resolved", "InProgress")]
    [InlineData("Closed", "InProgress")]
    [InlineData("Cancelled", "Open")]
    public async Task InvalidTransitions_ShouldBeRejected(string fromStatus, string toStatus)
    {
        var id = await CreateTicketAsync();
        await ReachStatusAsync(id, fromStatus);

        var response = await _client.PostAsJsonAsync($"/api/tickets/{id}/status", new { newStatus = toStatus });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}

