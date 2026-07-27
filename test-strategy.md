# Test Strategy

## Goals

- Prove the **status state machine** is enforced at the HTTP boundary (highest risk business rule).
- Smoke-test **Angular** list loading with mocked HTTP (fast, no flaky E2E).
- Rely on **manual E2E** for full create/detail/comment flows against local SQL Server.

## Backend integration tests

- **Project:** `tests/backend/SupportTicketManagement.Tests`
- **Harness:** `WebApplicationFactory<Program>` + EF InMemory in `Testing` environment.
- **Cases:**
  - Valid transitions: Open→InProgress, InProgress→Resolved, Resolved→Closed, Open→Cancelled, InProgress→Cancelled.
  - Invalid transitions: Open→Resolved, Open→Closed, Resolved→InProgress, Closed→InProgress, Cancelled→Open.
- **Helper:** `ReachStatusAsync` walks valid steps to reach the `from` state before asserting.

## Frontend unit tests

- `app.component.spec.ts` — shell header renders.
- `ticket-list.component.spec.ts` — `TicketService` GET mocked; verifies tickets populate after init.

## Not in scope

- Playwright/Cypress E2E (time-boxed exercise).
- Load or security penetration testing.

Results: `test-results.md`.
