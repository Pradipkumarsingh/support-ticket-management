# Cursor Spec / Master Prompt

Below is the main prompt I used with Cursor to build the **Support Ticket Management System** for the ".NET AI Capability Exercise – Project Option 1: Backend-Heavy Support Ticket Management System".

---

You are an expert .NET + Angular engineer helping me implement the ".NET AI Capability Exercise – Project Option 1: Backend-Heavy Support Ticket Management System".

I need you to DESIGN and IMPLEMENT a complete, small full-stack application plus all required lifecycle documentation, following the assessment guide. The stack and constraints are:

- Backend: ASP.NET Core .NET 8 Web API, Entity Framework Core, SQL Server
- Frontend: Angular 19 SPA
- Database: SQL Server instance on my machine (e.g. (localdb)\MSSQLLocalDB or SQLEXPRESS)
- Tests: xUnit integration tests for the ticket status state machine
- Tool: Cursor (you), used across the whole lifecycle, not just code generation

Please follow these rules throughout:
- Prefer clean, simple architecture (no over-engineering).
- Use clear naming, consistent formatting, and safe coding practices.
- No secrets or real passwords in code or config.
- Make the UI simple, modern, and responsive.
- Focus on Core requirements first; treat extras as Stretch.

---

## 1. Project option and business requirements

Project Option 1 – **Backend-Heavy Support Ticket Management System**.

**Entities**

- User (seeded only; no UI for CRUD)
  - id, name, email, role
- Ticket
  - id, title, description, priority, status, assignedTo, createdBy, createdAt, updatedAt
- Comment
  - id, ticketId, message, createdBy, createdAt

**Core features**

- Create a ticket.
- List tickets.
- View ticket detail.
- Update ticket fields (title, description, priority, assignee).
- Change ticket status through a strict state machine:
  - Open → In Progress
  - In Progress → Resolved
  - Resolved → Closed
  - Open → Cancelled
  - In Progress → Cancelled
- Add comments to a ticket.
- Keyword search and filter by status.
- Persist all data; data survives restart.
- Validate required fields and reject invalid input at the backend.
- Show meaningful error states in the UI.

**Status state machine (critical)**

This is the signature backend judgement piece. You must:
- Centralize the state machine logic (a dedicated service or method).
- Allow ONLY the valid transitions above.
- Reject any invalid transition (e.g., Open → Resolved, Closed → In Progress) with HTTP 400 and a clear error message.
- Frontend should react and show that error.

**Mandatory tests**

- Integration tests that prove:
  - Valid transitions succeed.
  - Invalid transitions are rejected.

---

## 2. Repository structure and files

Target structure (approx):

ai-practical-assessment/
- README.md
- candidate-info.md
- tool-workflow.md
- requirements-analysis.md
- acceptance-criteria.md
- implementation-plan.md
- design-notes.md
- api-contract.md
- data-model.md
- ui-flow.md
- test-strategy.md
- test-results.md
- debugging-notes.md
- code-review-notes.md
- review-fixes.md
- pr-description.md
- reflection.md
- final-ai-usage-summary.md
- src/
  - backend/SupportTicketManagement.Api/  (.NET 8 Web API)
  - frontend/                             (Angular 19 app)
- tests/
  - backend/SupportTicketManagement.Tests/  (xUnit integration tests)
- database/
  - schema-or-migrations/
  - seed-data/
  - setup-notes.md
- ai-prompts/
  - planning.md
  - design.md
  - implementation.md
  - testing.md
  - debugging.md
  - code-review.md
  - documentation.md
- tool-specific/
  - cursor-workflow/
    - project-context.md
    - spec.md
    - tasks.md
    - cursor-rules-or-instructions.md

Fill all markdown files with concise, meaningful content (no placeholders).

---

## 3. Backend design (ASP.NET Core .NET 8 Web API)

### 3.1 Projects and architecture

- Single API project: `SupportTicketManagement.Api` targeting `net8.0`.
- Use EF Core with SQL Server.
- Entities:
  - `User`, `Ticket`, `Comment`, plus enums `TicketPriority` and `TicketStatus`.
- `SupportTicketContext`:
  - DbSets for all entities.
  - Configure relationships and constraints.
  - Seed:
    - At least 5 users (Admin, Support, User roles).
    - A couple of sample tickets and comments.

### 3.2 Status state machine

Create `ITicketStatusService` with an implementation that:

- Defines allowed transitions:
  - Open → InProgress, Cancelled
  - InProgress → Resolved, Cancelled
  - Resolved → Closed
  - Closed, Cancelled → no transitions
- Method: `bool IsValidTransition(TicketStatus from, TicketStatus to)`.

Use it in the ticket status endpoint to enforce rules and return 400 on invalid transitions.

### 3.3 API endpoints

Implement controllers with DTOs (separate from EF entities). At minimum:

- `GET /api/users`
  - For assignee dropdown.
- `GET /api/tickets`
  - Query params: `search`, `status`.
  - Returns tickets with assignee/creator names, priority, status, timestamps.
- `GET /api/tickets/{id}`
  - Returns ticket + comments + user names.
- `POST /api/tickets`
  - Request: title, description, priority, createdByUserId, optional assignedToUserId.
  - Sets `Status = Open`, timestamps.
- `PUT /api/tickets/{id}`
  - Update title, description, priority, assignee.
- `POST /api/tickets/{id}/status`
  - Body: `{ newStatus: TicketStatus }`.
  - Enforce state machine with `ITicketStatusService`.
- `POST /api/tickets/{id}/comments`
  - Body: message, createdByUserId.

Validation rules:
- Title and Description required and non-empty.
- CreatedBy/AssignedTo must refer to existing users.
- Status changes must use the state machine.

Error handling:
- Use HTTP 400 for validation and invalid transitions (include error messages).
- 404 for missing tickets.
- 500 only for unexpected errors (log internally).

JSON serialization:
- Configure `JsonStringEnumConverter` so enums (Priority, Status) use string values in JSON.

### 3.4 CORS and Swagger

- Enable CORS to allow `http://localhost:4200`.
- Add Swagger (Swashbuckle.AspNetCore) and configure:
  - `AddEndpointsApiExplorer`, `AddSwaggerGen`.
  - `UseSwagger`, `UseSwaggerUI` in Development.
- Configure `launchSettings.json` to open Swagger at `/swagger` on run.

### 3.5 Database

- Use SQL Server.
- Provide:
  - EF Core migration `InitialCreate` for full schema and seed.
  - `database/setup-notes.md` describing:
    - DB name.
    - Example connection string.
    - How to run migrations (`dotnet ef`).
    - How to verify data persistence.
- Connection string configured via `appsettings.json` with placeholders only (no real secrets).

---

## 4. Frontend design (Angular 19)

### 4.1 App shell

- Root component:
  - Header: “Support Ticket Management” with gradient accent underline.
  - Nav: `Tickets` and `Create Ticket` links with active state styling.
  - Card-style content area with subtle shadow and rounded corners.
- Background: light gradient for a modern, colorful look.

### 4.2 Screens / routes

Use standalone components and Angular router:

- `''` → Ticket list (home).
- `'tickets/new'` → Create ticket.
- `'tickets/:id'` → Ticket detail.

### 4.3 Ticket list page

- Show:
  - Title, priority, status (with colored badges or text), assignee, updatedAt.
- Filters:
  - Text search (title/description) → `search` query param.
  - Status dropdown → `status` query param.
- States:
  - Loading spinner/text.
  - Error banner when API fails.
  - Empty state when no tickets.

### 4.4 Create ticket page

- Form fields:
  - Title (required).
  - Description (required).
  - Priority dropdown (Low, Medium, High).
  - Assignee dropdown populated from `/api/users`.
- On submit:
  - Call `POST /api/tickets`.
  - On success: navigate to ticket detail or list.
  - On backend error: show red error banner with message.

### 4.5 Ticket detail page

- Show:
  - Ticket title, id, status (badge), priority, assignee, created/updated timestamps.
  - Editable fields for title, description, priority, assignee (form with Save button).
- Status actions:
  - Show buttons for allowed transitions only (based on current status).
  - Calls `POST /api/tickets/{id}/status`.
  - On invalid transition (400) display clear message.
- Comments:
  - List existing comments (author name, timestamp, message).
  - Add comment form: textarea + button; calls `POST /api/tickets/{id}/comments`.

### 4.6 API services and configuration

- `APP_CONFIG` with `apiBaseUrl`, e.g. `https://localhost:7093`.
- `TicketService`:
  - `getTickets`, `getTicket`, `createTicket`, `updateTicket`, `changeStatus`, `addComment`.
- `UserService`:
  - `getUsers`.

### 4.7 Styling and responsiveness

- Use SCSS for:
  - Consistent button styles (primary, link).
  - Responsive layout for form rows (stack on small screens).
  - Colored status badges (e.g., blue for Open, orange for In Progress, green for Resolved, grey for Closed/Cancelled).
- Ensure layouts adapt for smaller viewports (mobile/tablet).

---

## 5. Tests

In `SupportTicketManagement.Tests` (xUnit):

- Use `WebApplicationFactory<Program>` for integration tests, with in-memory EF Core for isolation.
- Tests for **valid transitions**:
  - Start from Open and chain transitions as needed.
  - Assert 2xx and updated status from API.
- Tests for **invalid transitions**:
  - e.g., Open → Resolved, Closed → InProgress, Cancelled → Open.
  - Assert HTTP 400 and error message.

Record `dotnet test` results in `test-results.md`.

---

## 6. Documentation and AI workflow

For each markdown artifact:

- Explain decisions for backend, frontend, database, validation, and error handling.
- In `tool-workflow.md` and `tool-specific/cursor-workflow/*`, describe:
  - How I used Cursor for requirements analysis, planning, design, implementation, testing, debugging, and documentation.
  - How I provided project context (this prompt, repo structure, etc.).
  - How I validated and sometimes corrected AI-generated code.

Also:
- Populate `ai-prompts/*.md` with grouped prompt history:
  - Planning, design, implementation, testing, debugging, code-review, documentation.
  - For each: prompt (or summary), AI response summary, what I accepted/modified/rejected and why.

---

## 7. Working style for you (the AI)

- Work incrementally and tell me what you are doing at a high level.
- When you propose code, place it in the correct file path.
- Keep the status-machine logic and tests clear and central.
- Always respect the exercise constraints and repo structure.
