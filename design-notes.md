# Design Notes

## Architecture

- **Monolithic API** — Single ASP.NET Core Web API project with controllers, EF Core `DbContext`, and a small domain service for status rules.
- **SPA frontend** — Angular 19 standalone components; no NgRx; services call REST endpoints directly.
- **Persistence** — SQL Server in development; integration tests swap to EF InMemory via `WebApplicationFactory`.

## Status state machine

Enforced in `ITicketStatusService` / `TicketStatusService` (not in controllers alone):

| From | Allowed to |
|------|------------|
| Open | InProgress, Cancelled |
| InProgress | Resolved, Cancelled |
| Resolved | Closed |
| Closed | (none) |
| Cancelled | (none) |

Invalid transitions return HTTP 400 with an error payload.

## API shape

- REST resources under `/api/tickets` and `/api/users`.
- Enums serialized as strings in JSON (`JsonStringEnumConverter`) for Angular compatibility.
- Comments nested under tickets (`POST/GET` on ticket id).

## Frontend UX

- Global shell with nav links to list and create.
- List: search + status filter, loading/error/empty states.
- Detail: edit fields, status actions limited to allowed next states from API, comment thread.

## Trade-offs

- **No auth** — Simplifies exercise; `createdByUserId` chosen from seeded users in forms.
- **No pagination** — Acceptable for demo data volume.
- **DTOs inline in controller** — Keeps small codebase readable; would extract to separate files if the API grew.

See also `data-model.md` and `api-contract.md`.
