# AGENTS

This file guides agentic coding tools operating in this repo.
Follow these rules in addition to any system or tool instructions.

## Global principles
- Do not change public contracts without approval.
- Stop and ask if assumptions are unclear.
- Prefer small, focused changes.
- Always explain reasoning before large changes.

## Source of truth for frontend work (Next.js App Router)
These rules are mandatory when working in `src/HomeEconomics/frontend`.
- Read, in order: `architecture.md`, `coding-guidelines.md`, `specs/*.md`, `api-contracts.md`.
- Precedence: `architecture.md` overrides everything; `coding-guidelines.md` overrides specs.
- Implement only what the spec describes; no speculative work or unrelated refactors.
- No new dependencies unless explicitly approved and documented.

## Build, lint, and test commands
Run commands from the repo root unless noted.

### Backend (.NET 10)
- Restore: `dotnet restore`
- Build: `dotnet build`
- Run API: `dotnet run --project src/HomeEconomics`
- Run all tests: `dotnet test`
- Run a single test project: `dotnet test test/Domain.UnitTests`
- Run a single test by name:
  `dotnet test --filter FullyQualifiedName~Namespace.ClassName.MethodName`
- Run tests in a namespace:
  `dotnet test --filter FullyQualifiedName~Namespace.ClassName`

### Frontend (React SPA in `src/HomeEconomics/spa`)
- Install: `npm install`
- Dev server: `npm start`
- Build: `npm run build`
- Tests (watch): `npm test`
- Tests in CI mode: `npm run citest`
- Run a single test file or pattern:
  `npm test -- --watchAll=false <pattern>`
- Lint TSX: `npm run lint`
- Lint SCSS: `npm run stylelint`

### Frontend (Next.js App Router in `src/HomeEconomics/frontend`)
- Install: `npm install`
- Dev server: `npm run dev`
- Build: `npm run build`
- Start: `npm run start`
- Note: no lint/test scripts are currently defined.

## Code style and architecture
Follow existing patterns; consistency beats novelty.

### TypeScript (frontend Next.js)
- TypeScript `strict: true` and no `any`.
- Prefer explicit types for public APIs.
- One primary export per file.
- File names use `kebab-case`; components use `PascalCase`; hooks start with `use`.
- Source code in English; user-facing text in Spanish only.
- No new UI libraries; use MUI only with `sx` and theme tokens.
- No inline styles or per-component CSS files.
- Components are presentational by default; no direct API calls.
- Hooks own loading/error state and return UI-ready data; no JSX.
- Services do all `fetch` calls; no React imports or state.
- Services throw on non-2xx responses; hooks map errors to UI states.
- Errors must be user-visible; no silent failures.
- No `console.log` in committed code.

### React SPA (legacy, `src/HomeEconomics/spa`)
- Follow existing folder conventions and service patterns.
- API calls live in `services/*.service.ts` and shared HTTP helpers.
- Use TypeScript types consistent with backend DTOs.
- Prefer small, focused changes; avoid broad refactors.

### Backend (.NET)
- Clean Architecture and CQRS are mandatory.
- Keep business rules in domain entities.
- Commands/queries/handlers live together per feature.
- Validation via FluentValidation; edge validation rejects bad input.
- Errors follow ProblemDetails; use 409 for domain invariant violations.

## Imports and formatting
- Keep imports minimal and ordered by module type: external, internal, relative.
- Avoid circular dependencies; respect layer rules (app -> hooks -> services).
- Use existing formatting conventions; do not add a formatter unless approved.

## Error handling and logging
- Backend: throw domain errors intentionally; rely on middleware for ProblemDetails.
- Frontend: surface errors in UI; do not swallow exceptions.
- Remove temporary logs before committing.

## Tests and changes
- Test behavior, not implementation details.
- Avoid snapshot tests.
- When adding a feature, update docs if setup/usage changes.

## Cursor/Copilot rules
- No `.cursor/rules/`, `.cursorrules`, or `.github/copilot-instructions.md` found.
