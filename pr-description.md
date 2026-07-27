# Pull Request Description

## Summary

- Address assessor feedback: complete lifecycle documentation, AI prompt history, personalized candidate info, and checked acceptance criteria with evidence.
- Fix backend integration tests (in-memory seeding in `Testing` environment, status path helper, enum JSON in test payloads).
- Add root `.gitignore`, remove tracked build artifacts, improve Angular unit tests, align README with API URL.

## Test plan

- [ ] `dotnet build` API project
- [ ] `dotnet test tests/backend/SupportTicketManagement.Tests/SupportTicketManagement.Tests.csproj` (10 passed)
- [ ] `cd src/frontend && npm test -- --watch=false` (component specs)
- [ ] Manual: `dotnet run` API + `ng serve`, create ticket, comment, valid/invalid status change

## Files of note

- `Program.cs` — Testing environment user seed for integration tests
- `tests/.../TicketStatusIntegrationTests.cs` — `ReachStatusAsync` helper
- `acceptance-criteria.md`, `ai-prompts/`, `test-results.md`
