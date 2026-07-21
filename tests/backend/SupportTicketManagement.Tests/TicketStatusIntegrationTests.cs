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
            priority = 1,
            createdByUserId = 3,
            assignedToUserId = 2
        });

        response.EnsureSuccessStatusCode();

        var body = await response.Content.ReadFromJsonAsync<CreateTicketResponse>();
        return body!.Id;
    }

    private sealed record CreateTicketResponse(int Id);

    [Theory]
    [InlineData("Open", "InProgress")]
    [InlineData("InProgress", "Resolved")]
    [InlineData("Resolved", "Closed")]
    [InlineData("Open", "Cancelled")]
    [InlineData("InProgress", "Cancelled")]
    public async Task ValidTransitions_ShouldSucceed(string fromStatus, string toStatus)
    {
        var id = await CreateTicketAsync();

        if (!string.Equals(fromStatus, "Open", StringComparison.OrdinalIgnoreCase))
        {
            var first = await _client.PostAsJsonAsync($"/api/tickets/{id}/status", new { newStatus = fromStatus });
            first.EnsureSuccessStatusCode();
        }

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

        if (!string.Equals(fromStatus, "Open", StringComparison.OrdinalIgnoreCase))
        {
            var first = await _client.PostAsJsonAsync($"/api/tickets/{id}/status", new { newStatus = fromStatus });
            first.EnsureSuccessStatusCode();
        }

        var response = await _client.PostAsJsonAsync($"/api/tickets/{id}/status", new { newStatus = toStatus });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}

