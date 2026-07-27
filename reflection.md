# Reflection

## What went well

- Using Cursor to scaffold the API and Angular routes quickly let me focus on the state machine and persistence.
- Capturing debugging steps helped when the same CORS/port issues appeared again.
- Integration tests gave confidence that invalid transitions cannot bypass the service layer.

## What was challenging

- Aligning Kestrel ports, Angular `config.ts`, and documentation took several iterations.
- EF InMemory + `WebApplicationFactory` seeding was subtle; fixing it in `Program.cs` for `Testing` was the reliable approach.
- Balancing exercise scope with documentation expectations — many markdown files are required for the assessment rubric.

## What I would do differently

- Add root `.gitignore` before the first commit to avoid tracking `obj/` and `bin/`.
- Run `dotnet test` in CI (or pre-commit) from day one.
- Keep `ai-prompts/` updated after each major AI session instead of at the end.

## AI usage honesty

AI generated substantial boilerplate (entities, controllers, Angular components). I reviewed all changes, ran the app, fixed integration issues manually, and made product decisions (no auth, status rules, SQL Server connection).
