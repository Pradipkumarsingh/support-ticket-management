# Debugging prompts (iterative)

## API connectivity

> Angular cannot load tickets. Network tab shows connection refused. Backend launchSettings uses https://localhost:7093. Update config and README.

## CORS

> Browser error: CORS policy blocked fetch from localhost:4200. Add ASP.NET Core CORS for the Angular origin.

## Create ticket 400

> POST /api/tickets returns 400 from UI but works in Swagger with same body. Check enum serialization for priority.

## Tests failing

> dotnet test fails with CreatedBy user does not exist on WebApplicationFactory in-memory database. Seed users when environment is Testing and use string enum values in test JSON.

**Outcome:** Changes documented in `debugging-notes.md`; fixes in `Program.cs`, `config.ts`, integration tests.
