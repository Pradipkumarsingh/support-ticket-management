# Acceptance Criteria

## Core

- [ ] A user can **create a ticket** via the Angular UI.
- [ ] A user can **view all tickets** from the SQL Server database.
- [ ] A user can **open a ticket detail view** showing full fields and comments.
- [ ] A user can **update ticket fields** (title, description, priority, assignee).
- [ ] A user can **add comments** to a ticket.
- [ ] **Status changes** only occur through valid transitions and are enforced by the backend.
- [ ] **Keyword search** and **status filter** are available on the ticket list.
- [ ] Data **persists after restart** of backend/frontend.

## Validation

- [ ] Backend rejects creating or updating tickets with missing required fields.
- [ ] Backend rejects assigning tickets to non-existent users.
- [ ] Backend validates comment messages are non-empty.
- [ ] Status change endpoint validates allowed transitions only.

## Error Handling

- [ ] Backend returns appropriate HTTP status codes:
  - 400 for validation errors and invalid transitions.
  - 404 for missing tickets.
  - 500 only for unexpected failures, with generic messages.
- [ ] Frontend shows:
  - Loading state for list and detail views.
  - Empty state when there are no tickets or comments.
  - Error state when API requests fail or validation errors occur.

## Testing

- [ ] Integration tests validate **valid status transitions succeed**.
- [ ] Integration tests validate **invalid status transitions are rejected with 400**.
- [ ] Frontend tests cover the **create → list → detail** flow at least at component level (or with mocked services).

## Documentation

- [ ] `README.md` documents setup and run instructions for backend, frontend, and database.
- [ ] `database/setup-notes.md` explains schema, seed data, and persistence verification.
- [ ] Lifecycle markdown files (requirements, design, implementation plan, test strategy, debugging notes, code review, reflection) are filled with meaningful content.
- [ ] AI usage and prompts are captured under `ai-prompts/` and `tool-specific/`.

