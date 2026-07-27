# Tool Workflow

## Primary tool

**Cursor** (Agent mode) for planning, implementation, debugging, and documentation.

## How I used AI in this project

1. **Kickoff** — Pasted the assessment brief and asked for a structured plan aligned to Option 1 (backend-heavy tickets).
2. **Incremental build** — Backend entities and API first, then Angular screens, then EF migrations and seed data.
3. **Debug loops** — When the UI failed (CORS, wrong port, enum JSON binding), I described console/network errors and applied targeted fixes.
4. **Review pass** — Used feedback on missing lifecycle docs and test reliability to add artifacts and fix integration test seeding.

## Human decisions (not delegated to AI)

- SQL Server instance and connection string for local machine.
- Skipping optional authentication per exercise scope.
- Status state machine rules (Open → InProgress/Cancelled, etc.).
- Final verification of acceptance criteria before submission.

## Related artifacts

- `tool-specific/cursor-workflow/spec.md` — master prompt / context for Cursor.
- `ai-prompts/` — representative prompts by phase.
- `tool-workflow.md` (this file) — high-level process.
- `final-ai-usage-summary.md` — reflection on what worked and what I would change.
