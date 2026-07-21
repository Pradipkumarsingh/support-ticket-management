using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SupportTicketManagement.Api.Data;

namespace SupportTicketManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(SupportTicketContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetUsers(CancellationToken cancellationToken)
    {
        var users = await db.Users
            .AsNoTracking()
            .Select(u => new
            {
                u.Id,
                u.Name,
                u.Email,
                u.Role
            })
            .ToListAsync(cancellationToken);

        return Ok(users);
    }
}

