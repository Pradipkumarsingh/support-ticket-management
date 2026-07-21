# AI Practical Assessment – Support Ticket Management System

This repository contains my submission for the **.NET AI Capability Exercise – Project Option 1: Backend-Heavy Support Ticket Management System**.

The application is a small full-stack system that allows internal users to create, view, update, search, and comment on support tickets, with an enforced backend status state machine.

## Tech Stack

- **Backend**: ASP.NET Core **.NET 8** Web API, Entity Framework Core, SQL Server
- **Frontend**: **Angular 19** SPA
- **Database**: Microsoft SQL Server (LocalDB / Developer / Express)

## Repository Structure

- `candidate-info.md` – basic candidate and project info
- Core lifecycle docs:
  - `requirements-analysis.md`, `acceptance-criteria.md`, `implementation-plan.md`, `design-notes.md`
  - `api-contract.md`, `data-model.md`, `ui-flow.md`
  - `test-strategy.md`, `test-results.md`, `debugging-notes.md`
  - `code-review-notes.md`, `review-fixes.md`, `pr-description.md`
  - `reflection.md`, `final-ai-usage-summary.md`
- `src/backend/SupportTicketManagement.Api` – .NET 8 Web API
- `src/frontend` – Angular 19 application
- `tests/backend/SupportTicketManagement.Tests` – backend integration tests
- `database/` – SQL scripts and setup notes
- `ai-prompts/` – grouped AI prompt history
- `tool-specific/cursor-workflow/` – Cursor-specific context and workflow notes

## Backend Setup (.NET 8 Web API)

1. **Prerequisites**
   - .NET 8 SDK installed
   - SQL Server instance (e.g., `(localdb)\MSSQLLocalDB`)

2. **Configure Connection String**
   - Open `src/backend/SupportTicketManagement.Api/appsettings.json`.
   - Update `DefaultConnection` if needed, e.g.:

   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=(localdb)\\\\MSSQLLocalDB;Database=SupportTicketDb;Trusted_Connection=True;TrustServerCertificate=True"
   }
   ```

3. **Create Database Schema and Seed Data**
   - See `database/setup-notes.md` for commands.
   - In summary:
     - Create database `SupportTicketDb`.
     - Run `database/schema-or-migrations/initial-schema.sql`.
     - Run `database/seed-data/seed-data.sql`.

4. **Run the API**

```powershell
cd ai-practical-assessment/src/backend/SupportTicketManagement.Api
dotnet run
```

The API will start on the configured Kestrel ports (check console output). You should be able to hit `GET /api/tickets` and `GET /api/users`.

## Frontend Setup (Angular 19)

1. **Prerequisites**
   - Node.js (LTS)
   - npm

2. **Install Dependencies**

```powershell
cd ai-practical-assessment/src/frontend
npm install
```

3. **Configure API Base URL**

The Angular app reads the API base URL from `src/app/config.ts`:

```ts
export const APP_CONFIG = {
  apiBaseUrl: 'https://localhost:5001'
};
```

Update `apiBaseUrl` to match your backend API URL (e.g., `https://localhost:5001` or `http://localhost:5084`).

4. **Run the Angular Dev Server**

```powershell
cd ai-practical-assessment/src/frontend
npm start
```

or

```powershell
npx ng serve
```

The app will run at `http://localhost:4200/` by default.

## End-to-End Flow to Verify

1. Start SQL Server and prepare the database (schema + seed scripts).
2. Run the backend API (`dotnet run`).
3. Run the Angular dev server (`ng serve`).
4. In the browser:
   - Open the ticket list and see seeded tickets.
   - Use search and status filter to narrow results.
   - Create a new ticket via the **Create Ticket** screen.
   - Open a ticket detail, edit fields, and save changes.
   - Add comments and verify they appear in the list.
   - Change ticket status via the allowed actions and verify:
     - Valid transitions succeed.
     - Invalid transitions (e.g., `Open -> Resolved`) are rejected with a clear error.

## Tests

### Backend

From the repo root:

```powershell
cd ai-practical-assessment
dotnet test tests/backend/SupportTicketManagement.Tests/SupportTicketManagement.Tests.csproj
```

The integration tests focus on verifying that valid status transitions succeed and invalid ones are rejected with HTTP 400 responses.

### Frontend

You can run the default Angular test runner:

```powershell
cd ai-practical-assessment/src/frontend
npm test
```

Additional notes about test scope and results are in `test-strategy.md` and `test-results.md`.

## Notes

- No real secrets (passwords, API keys) are committed; connection strings use local SQL Server with trusted connections only.
- The state machine logic lives in a dedicated backend service and is tested through HTTP-level integration tests.
- AI usage, prompt history, and workflow design are documented under `ai-prompts/` and `tool-specific/`.

