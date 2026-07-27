# Code Review Notes

Self-review before submission (and after assessor feedback).

## Strengths

- Clear separation of status rules in `TicketStatusService`.
- Controllers validate users and required fields before writes.
- Angular list/detail expose loading and error states.
- Integration tests cover the state machine at HTTP level.

## Gaps addressed in review pass

- Lifecycle markdown files were missing or placeholders — added full set.
- `candidate-info.md` and `acceptance-criteria.md` not personalized — updated with name, dates, checked criteria.
- `ai-prompts/` empty — added phase-based prompt summaries.
- Repository tracked `bin/`, `obj/`, `.vs/` — added root `.gitignore` and removed build artifacts from index.
- Integration tests failed locally — fixed test DB seeding and transition path helper.
- Default Angular spec still expected old CLI template — updated specs.

## Remaining improvements (out of scope for time-box)

- Extract request DTOs to dedicated files.
- Add pagination on ticket list.
- Optional JWT auth if product requires it.
- E2E tests (Playwright) for full UI flows.

See `review-fixes.md` for the checklist mapped to feedback.
