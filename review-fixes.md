# Review Fixes

Checklist of assessor feedback items and resolution.

| Feedback area | Action taken |
|---------------|--------------|
| Placeholder candidate info | Filled `candidate-info.md` with Pradip Kumar Singh and dates |
| Unchecked acceptance criteria | Marked `[x]` with verification notes in `acceptance-criteria.md` |
| Missing lifecycle docs | Added `design-notes.md`, `api-contract.md`, `data-model.md`, `ui-flow.md`, `test-strategy.md`, `test-results.md`, `debugging-notes.md`, `code-review-notes.md`, `tool-workflow.md`, `reflection.md`, `final-ai-usage-summary.md`, `pr-description.md` |
| No `ai-prompts/` history | Created `ai-prompts/` with planning, implementation, debugging, review prompts |
| Tests not passing / not visible | Fixed integration test seeding; tests remain under `tests/backend/`; removed template `UnitTest1.cs` |
| `obj/` / `bin/` in repo | Root `.gitignore`; `git rm --cached` for tracked build output |
| Weak frontend tests | Updated `app.component.spec.ts`; added `ticket-list.component.spec.ts` |
| README API URL mismatch | Documented `https://localhost:7093` |
| Only `spec.md` for AI workflow | Linked from `tool-workflow.md` and `ai-prompts/` |

Commit: `cursor/feedback-documentation-fixes` branch.
