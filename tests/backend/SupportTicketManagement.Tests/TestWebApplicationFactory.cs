using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SupportTicketManagement.Api.Data;

namespace SupportTicketManagement.Tests;

public class TestWebApplicationFactory : WebApplicationFactory<Program>
{
    private const string DatabaseName = "SupportTicketTests";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<SupportTicketContext>));

            if (descriptor is not null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<SupportTicketContext>(options =>
                options.UseInMemoryDatabase(DatabaseName));
        });
    }
}
