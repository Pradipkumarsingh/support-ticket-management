# Implementation prompts (representative)

## Backend

> Add entities User, Ticket, Comment with TicketPriority and TicketStatus enums. Implement SupportTicketContext with SQL Server, seed five users and sample tickets. Add TicketsController with list (search + status filter), get by id, create, update, POST status, POST comments. Enforce status transitions in ITicketStatusService.

## Frontend

> Create Angular 19 standalone routes for ticket list, create, and detail. Use TicketService with HttpClient. Show loading, error, and empty states on list and detail.

## Follow-up

> Add CORS for http://localhost:4200 and JsonStringEnumConverter for priority/status in API JSON.

**Outcome:** `src/backend/`, `src/frontend/`, migrations under `Migrations/`.
