# Debugging Notes

Issues encountered during development and how they were resolved.

## Frontend could not reach API

**Symptom:** Network errors / empty list in Angular.  
**Cause:** `apiBaseUrl` pointed at wrong port (`5001` vs Kestrel `7093`).  
**Fix:** Set `src/app/config.ts` to `https://localhost:7093` to match `launchSettings.json`.

## CORS blocked browser calls

**Symptom:** Browser console CORS errors from `http://localhost:4200`.  
**Fix:** `AddCors` policy allowing `http://localhost:4200` and `UseCors()` in `Program.cs`.

## Create ticket returned 400

**Symptom:** POST `/api/tickets` failed after UI worked in Swagger.  
**Cause:** `TicketPriority` sent as string; API needed `JsonStringEnumConverter` for enum binding.  
**Fix:** Added converter in `AddJsonOptions` in `Program.cs`.

## Port already in use

**Symptom:** API failed to bind to `5155`.  
**Cause:** Second instance (CLI + Visual Studio).  
**Fix:** Run only one API host at a time.

## Swagger not opening

**Fix:** `launchUrl: swagger` in `launchSettings.json`; Swashbuckle in Development.

## EF migrations CLI (PowerShell)

**Symptom:** Multi-line command with backticks failed.  
**Fix:** Single-line `dotnet ef migrations add ... --project ... --startup-project ...`.

## Integration tests: CreatedBy user does not exist

**Symptom:** All `TicketStatusIntegrationTests` failed on create.  
**Cause:** In-memory DB not seeded in the same pipeline as test HTTP client; enum JSON in tests needed string priority.  
**Fix:** Seed users when `Environment` is `Testing` in `Program.cs`; fixed `ReachStatusAsync` paths; use `priority: "Medium"` in test payloads.

## Git push SSH

**Symptom:** Host key verification failed.  
**Fix:** Use HTTPS remote URL for GitHub.
