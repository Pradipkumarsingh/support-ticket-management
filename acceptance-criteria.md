# Acceptance Criteria

Verification notes reference manual UI checks (Angular + API running locally) and automated backend tests unless noted.

## Core

- [x] A user can **create a ticket** via the Angular UI. — Verified on Create Ticket form (`/tickets/new`); POST `/api/tickets` returns 201.
- [x] A user can **view all tickets** from the SQL Server database. — List loads from GET `/api/tickets`; seed data visible after migration.
- [x] A user can **open a ticket detail view** showing full fields and comments. — Route `/tickets/:id`; GET `/api/tickets/{id}` includes comments.
- [x] A user can **update ticket fields** (title, description, priority, assignee). — Detail form PUT `/api/tickets/{id}`.
- [x] A user can **add comments** to a ticket. — POST `/api/tickets/{id}/comments`.
- [x] **Status changes** only occur through valid transitions and are enforced by the backend. — `TicketStatusService` + integration tests in `TicketStatusIntegrationTests.cs`.
- [x] **Keyword search** and **status filter** are available on the ticket list. — Query params `search` and `status` on GET `/api/tickets`; UI filters on list page.
- [x] Data **persists after restart** of backend/frontend. — SQL Server + EF migrations; documented in `database/setup-notes.md`.

## Validation

- [x] Backend rejects creating or updating tickets with missing required fields. — 400 with message for empty title/description.
- [x] Backend rejects assigning tickets to non-existent users. — 400 `AssignedTo user does not exist.`
- [x] Backend validates comment messages are non-empty. — 400 on empty comment body.
- [x] Status change endpoint validates allowed transitions only. — 400 on invalid transition; covered by integration tests.

## Error Handling

- [x] Backend returns appropriate HTTP status codes:
  - 400 for validation errors and invalid transitions.
  - 404 for missing tickets.
  - 500 only for unexpected failures, with generic messages.
- [x] Frontend shows:
  - Loading state for list and detail views. — `loading` signals in list/detail components.
  - Empty state when there are no tickets or comments. — Templates when arrays are empty.
  - Error state when API requests fail or validation errors occur. — `error` signals and user-facing messages.

## Testing

- [x] Integration tests validate **valid status transitions succeed**. — `ValidTransitions_ShouldSucceed` (5 cases).
- [x] Integration tests validate **invalid status transitions are rejected with 400**. — `InvalidTransitions_ShouldBeRejected` (5 cases).
- [x] Frontend tests cover the **create → list → detail** flow at least at component level (or with mocked services). — `ticket-list.component.spec.ts` (list load); `app.component.spec.ts` (shell); create/detail exercised manually per `test-strategy.md`.

## Documentation

- [x] `README.md` documents setup and run instructions for backend, frontend, and database.
- [x] `database/setup-notes.md` explains schema, seed data, and persistence verification.
- [x] Lifecycle markdown files (requirements, design, implementation plan, test strategy, debugging notes, code review, reflection) are filled with meaningful content.
- [x] AI usage and prompts are captured under `ai-prompts/` and `tool-specific/`.
