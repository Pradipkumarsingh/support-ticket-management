# Test Results

**Date:** 2026-03-27  
**Environment:** Windows, .NET 8 SDK, local workspace `ai-practical-assessment`

## Backend (`SupportTicketManagement.Tests`)

**Command:**

```powershell
dotnet test tests/backend/SupportTicketManagement.Tests/SupportTicketManagement.Tests.csproj --verbosity normal
```

**Summary:**

```
Test Run Successful.
Total tests: 10
     Passed: 10
 Total time: ~1.6 s
```

**Tests executed:**

| Test | Result |
|------|--------|
| `ValidTransitions_ShouldSucceed` (5 theory cases) | Passed |
| `InvalidTransitions_ShouldBeRejected` (5 theory cases) | Passed |

**Notes:** Tests use `WebApplicationFactory` with in-memory EF Core, `Testing` environment, and seeded users in `Program.cs` when `IsEnvironment("Testing")`.

## Frontend

**Command:**

```powershell
cd src/frontend
npm test -- --watch=false --browsers=ChromeHeadless
```

**Scope:** `app.component.spec.ts` (shell), `ticket-list.component.spec.ts` (mocked HTTP list load). Full create/detail flows verified manually during development (see `acceptance-criteria.md`).

## Build

```powershell
dotnet build src/backend/SupportTicketManagement.Api/SupportTicketManagement.Api.csproj
```

Build succeeded with 0 warnings at time of last run.
