using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SupportTicketManagement.Api.Data;
using SupportTicketManagement.Api.Models;
using SupportTicketManagement.Api.Services;

namespace SupportTicketManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TicketsController(SupportTicketContext db, ITicketStatusService statusService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetTickets(
        [FromQuery] string? search,
        [FromQuery] TicketStatus? status,
        CancellationToken cancellationToken)
    {
        var query = db.Tickets
            .Include(t => t.AssignedToUser)
            .Include(t => t.CreatedByUser)
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(t =>
                t.Title.Contains(term) ||
                t.Description.Contains(term));
        }

        if (status.HasValue)
        {
            query = query.Where(t => t.Status == status.Value);
        }

        var tickets = await query
            .OrderByDescending(t => t.UpdatedAt)
            .Select(t => new
            {
                t.Id,
                t.Title,
                t.Description,
                Priority = t.Priority.ToString(),
                Status = t.Status.ToString(),
                AssignedTo = t.AssignedToUser != null ? t.AssignedToUser.Name : null,
                CreatedBy = t.CreatedByUser != null ? t.CreatedByUser.Name : null,
                t.CreatedAt,
                t.UpdatedAt
            })
            .ToListAsync(cancellationToken);

        return Ok(tickets);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetTicketById(int id, CancellationToken cancellationToken)
    {
        var ticket = await db.Tickets
            .Include(t => t.AssignedToUser)
            .Include(t => t.CreatedByUser)
            .Include(t => t.Comments)
                .ThenInclude(c => c.CreatedByUser)
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

        if (ticket == null)
        {
            return NotFound();
        }

        var result = new
        {
            ticket.Id,
            ticket.Title,
            ticket.Description,
            Priority = ticket.Priority.ToString(),
            Status = ticket.Status.ToString(),
            AssignedToUserId = ticket.AssignedToUserId,
            AssignedTo = ticket.AssignedToUser?.Name,
            CreatedByUserId = ticket.CreatedByUserId,
            CreatedBy = ticket.CreatedByUser?.Name,
            ticket.CreatedAt,
            ticket.UpdatedAt,
            Comments = ticket.Comments
                .OrderBy(c => c.CreatedAt)
                .Select(c => new
                {
                    c.Id,
                    c.Message,
                    c.CreatedAt,
                    c.CreatedByUserId,
                    CreatedBy = c.CreatedByUser != null ? c.CreatedByUser.Name : null
                })
        };

        return Ok(result);
    }

    public record CreateTicketRequest(
        string Title,
        string Description,
        TicketPriority Priority,
        int CreatedByUserId,
        int? AssignedToUserId);

    public record UpdateTicketRequest(
        string Title,
        string Description,
        TicketPriority Priority,
        int? AssignedToUserId);

    public record ChangeStatusRequest(TicketStatus NewStatus);

    public record AddCommentRequest(
        string Message,
        int CreatedByUserId);

    [HttpPost]
    public async Task<IActionResult> CreateTicket([FromBody] CreateTicketRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        if (string.IsNullOrWhiteSpace(request.Title) || string.IsNullOrWhiteSpace(request.Description))
        {
            return BadRequest(new { error = "Title and Description are required." });
        }

        var creator = await db.Users.FindAsync([request.CreatedByUserId], cancellationToken);
        if (creator == null)
        {
            return BadRequest(new { error = "CreatedBy user does not exist." });
        }

        User? assignee = null;
        if (request.AssignedToUserId.HasValue)
        {
            assignee = await db.Users.FindAsync([request.AssignedToUserId.Value], cancellationToken);
            if (assignee == null)
            {
                return BadRequest(new { error = "AssignedTo user does not exist." });
            }
        }

        var now = DateTime.UtcNow;
        var ticket = new Ticket
        {
            Title = request.Title.Trim(),
            Description = request.Description.Trim(),
            Priority = request.Priority,
            Status = TicketStatus.Open,
            CreatedByUserId = request.CreatedByUserId,
            AssignedToUserId = request.AssignedToUserId,
            CreatedAt = now,
            UpdatedAt = now
        };

        db.Tickets.Add(ticket);
        await db.SaveChangesAsync(cancellationToken);

        return CreatedAtAction(nameof(GetTicketById), new { id = ticket.Id }, new { ticket.Id });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateTicket(int id, [FromBody] UpdateTicketRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var ticket = await db.Tickets.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        if (ticket == null)
        {
            return NotFound();
        }

        if (string.IsNullOrWhiteSpace(request.Title) || string.IsNullOrWhiteSpace(request.Description))
        {
            return BadRequest(new { error = "Title and Description are required." });
        }

        User? assignee = null;
        if (request.AssignedToUserId.HasValue)
        {
            assignee = await db.Users.FindAsync([request.AssignedToUserId.Value], cancellationToken);
            if (assignee == null)
            {
                return BadRequest(new { error = "AssignedTo user does not exist." });
            }
        }

        ticket.Title = request.Title.Trim();
        ticket.Description = request.Description.Trim();
        ticket.Priority = request.Priority;
        ticket.AssignedToUserId = request.AssignedToUserId;
        ticket.UpdatedAt = DateTime.UtcNow;

        await db.SaveChangesAsync(cancellationToken);

        return NoContent();
    }

    [HttpPost("{id:int}/status")]
    public async Task<IActionResult> ChangeStatus(int id, [FromBody] ChangeStatusRequest request, CancellationToken cancellationToken)
    {
        var ticket = await db.Tickets.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        if (ticket == null)
        {
            return NotFound();
        }

        if (!statusService.IsValidTransition(ticket.Status, request.NewStatus))
        {
            return BadRequest(new
            {
                error = $"Invalid status transition from {ticket.Status} to {request.NewStatus}."
            });
        }

        ticket.Status = request.NewStatus;
        ticket.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync(cancellationToken);

        return Ok(new { ticket.Id, Status = ticket.Status.ToString() });
    }

    [HttpPost("{id:int}/comments")]
    public async Task<IActionResult> AddComment(int id, [FromBody] AddCommentRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Message))
        {
            return BadRequest(new { error = "Comment message is required." });
        }

        var ticket = await db.Tickets.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        if (ticket == null)
        {
            return NotFound();
        }

        var user = await db.Users.FindAsync([request.CreatedByUserId], cancellationToken);
        if (user == null)
        {
            return BadRequest(new { error = "CreatedBy user does not exist." });
        }

        var comment = new Comment
        {
            TicketId = ticket.Id,
            Message = request.Message.Trim(),
            CreatedByUserId = request.CreatedByUserId,
            CreatedAt = DateTime.UtcNow
        };

        db.Comments.Add(comment);
        await db.SaveChangesAsync(cancellationToken);

        return Ok(new { comment.Id });
    }
}

