# Requirement Analysis

## Selected Project Option

Option 1 – **Backend-Heavy Support Ticket Management System**.

## My Understanding (in my own words)

I need to build a small support ticket system where internal users can create, view, update, search, and comment on tickets. Each ticket moves through a controlled lifecycle enforced by a backend status state machine, and all data (users, tickets, comments) is persisted in a SQL Server database. The frontend should surface core flows (create, list, detail, update, change status, comment) with clear loading, error, and empty states.

## Functional Requirements

- Manage **Users** (seeded only, no UI): id, name, email, role.
- Manage **Tickets**:
  - Create tickets with title, description, priority, status, assignee, createdBy, timestamps.
  - List tickets with keyword search and status filter.
  - View ticket details including metadata and comments.
  - Update ticket fields: title, description, priority, assignee.
  - Change ticket status following the defined state machine.
- Manage **Comments**:
  - Add comments to a ticket.
  - View comments in ticket detail.
- Persist all data so it survives application restarts.
- Provide at least one **search/filter** capability (keyword + status).
- Enforce backend **input validation** and return clear errors.

## Non-Functional Requirements

- Simple, clean architecture suitable for a small codebase.
- Clear separation between domain models and API DTOs.
- Meaningful error handling and HTTP status codes.
- Tests demonstrating the correctness of the status state machine.
- No secrets committed into source control.
- Reasonable performance for small data sets (no complex perf requirements).

## Assumptions

- Authentication is **not required** for Core; users are represented as seeded records referenced by id.
- A single tenant is sufficient (no per-company isolation).
- Only basic ticket fields are needed; no attachments, SLAs, or advanced workflows.
- One environment (local development) is enough for this exercise; deployment is out of scope.
- Time zones are handled via UTC timestamps in the backend, formatted by the frontend.

## Clarifications (questions for a product owner)

- Should priorities be fixed to Low/Medium/High or configurable?
- Are there any role-based restrictions on who can change statuses or edit tickets?
- Should comments be editable or deletable, or treated as an immutable audit trail?
- Do we need pagination for the ticket list once the number of tickets grows?
- Any specific constraints on the maximum length of titles/descriptions?

## Edge Cases

- Creating a ticket with missing required fields (title, description, createdBy).
- Assigning a ticket to a non-existent user id.
- Changing status using an invalid transition (e.g., `Open -> Resolved`).
- Changing the status of a ticket that is already `Closed` or `Cancelled`.
- Adding a comment to a non-existent ticket or with an empty message.
- Searching with empty or whitespace-only terms.
- Handling database connectivity issues gracefully on the API side.

