# Implementation Plan

## Overview

Implement a small support ticket management system with:
- **Backend**: ASP.NET Core .NET 8 Web API + EF Core + SQL Server.
- **Frontend**: Angular 19 SPA.
- **Database**: SQL Server with schema and seed data scripts.
- **Tests**: Integration tests focused on the ticket status state machine.

## Task Breakdown

1. **Project and Repo Scaffolding**
   - Create `ai-practical-assessment/` structure and markdown artifact files.
   - Scaffold .NET Web API and Angular 19 projects under `src/`.
2. **Domain & Data Model**
   - Define `User`, `Ticket`, `Comment` entities and enums for `TicketPriority` and `TicketStatus`.
   - Configure `SupportTicketContext` with EF Core and SQL Server, plus seed data.
3. **Backend API**
   - Implement controllers and DTOs for:
     - `POST /api/tickets`, `GET /api/tickets`, `GET /api/tickets/{id}`,
     - `PUT /api/tickets/{id}`, `POST /api/tickets/{id}/status`,
     - `POST /api/tickets/{id}/comments`, `GET /api/users`.
   - Add input validation and consistent error responses.
4. **Status State Machine**
   - Implement a dedicated `TicketStatusService` to enforce valid transitions.
   - Integrate this service in the status-change endpoint.
5. **Frontend (Angular 19)**
   - Build standalone components for ticket list, create, and detail (with comments and status actions).
   - Implement `TicketService` and `UserService` to call the backend.
   - Handle loading, empty, and error states on all major views.
6. **Testing**
   - Write integration tests to assert valid/invalid status transitions using `TestWebApplicationFactory`.
   - Add a basic frontend test tier (component tests or service tests).
7. **Documentation & AI Artifacts**
   - Fill all required markdown files (requirements, design, test strategy, debugging, code review, reflection).
   - Capture AI prompt history in `ai-prompts/` and Cursor-specific workflow notes.

## Milestones

1. Backend API with working SQL Server persistence and state machine.
2. Angular UI wired to backend for full create → list → detail → comment → status change flow.
3. Integration tests passing for status rules.
4. All documentation files drafted and refined.

## AI Usage Plan

- Use AI (Cursor) to:
  - Refine requirements and acceptance criteria.
  - Propose initial backend and frontend architectures.
  - Generate boilerplate code (entities, DTOs, controllers, Angular components).
  - Suggest test cases and example integration tests for the state machine.
  - Review code for edge cases and possible simplifications.
- Explicitly:
  - Validate AI output against the exercise spec and .NET/Angular docs.
  - Adjust generated code to match my own understanding and style.

## Risks

- **Framework version mismatches** (e.g., .NET 10 preview vs .NET 8 stable).
- **Database connection issues** (e.g., local SQL Server not running, incorrect connection string).
- **Over-scoping** the UI or backend features beyond Core requirements.
- **Time spent on styling** instead of lifecycle artifacts and tests.

## Mitigation

- Pin to **.NET 8** in all csproj files and test restore/build early.
- Keep SQL Server connection strings local and simple, and verify with a minimal query.
- Focus first on Core flows and tests; defer any “nice-to-have” stretch features.
- Use simple, clean UI styling; prioritize clarity and states (loading/empty/error) over visual polish.

