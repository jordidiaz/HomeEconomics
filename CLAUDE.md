# HomeEconomics – Claude Code Context

## What this project is
Personal finance tracker. Tracks monthly income/expenses, payment status, and recurring movements.

## Tech stack
- **Backend**: ASP.NET Core (.NET 10), Clean Architecture + CQRS (LiteBus), EF Core, PostgreSQL
- **Frontend**: Next.js 14 (App Router), TypeScript strict, MUI
- **Testing**: xUnit + FluentAssertions (backend), Vitest + RTL + Playwright (frontend)

## Project structure
```
src/
├── Domain/                       # Entities, business rules
├── Persistence/                  # EF Core, DbContext, configurations
└── HomeEconomics/                # ASP.NET Core API
    ├── Features/                 # CQRS: command/query/handler/validator per feature
    ├── Extensions/               # Service registration, middleware
    └── frontend/                 # Next.js App Router frontend
        ├── app/                  # Routes, layouts, pages
        ├── components/           # Presentational UI components
        ├── hooks/                # State and side effects
        ├── services/             # API access layer
        └── types/                # Shared TypeScript types
docs/
├── architecture.md               # Backend architecture and patterns
├── api.md                        # API reference
└── frontend/                     # Frontend detailed docs (read when doing frontend work)
    ├── architecture.md           # Mandatory frontend architecture rules
    ├── coding-guidelines.md      # TypeScript/component/hook/service conventions
    ├── api-contracts.md          # API behavioral contracts and invariants
    └── testing-strategy.md       # Test layers, scope, and coverage targets
```

## Commands

### Backend (run from repo root)
```
dotnet restore
dotnet build
dotnet run --project src/HomeEconomics
dotnet test
dotnet test test/Domain.UnitTests
dotnet test --filter FullyQualifiedName~Namespace.ClassName.MethodName  # single test
dotnet test --filter FullyQualifiedName~Namespace.ClassName             # tests in class
```

### Frontend (run from `src/HomeEconomics/frontend/`)
```
npm install
npm run dev
npm run build
npm test           # unit + component + integration (Vitest)
npm run test:ci    # single-run, no watch
npm run e2e        # Playwright E2E against local backend
```

## Key architecture rules
- **Backend**: Clean Architecture is mandatory. Business rules stay in `Domain/`. Commands/queries/handlers live together per feature in `Features/`.
- **Frontend**: Data flows one way — `app/ → hooks/ → services/ → API`. Reverse is forbidden. See `docs/frontend/architecture.md`.
- **No new dependencies** without explicit approval.
- **User-facing text in Spanish only.**
- **Import ordering**: external packages → internal modules → relative paths; no circular dependencies.

## Mandatory: testing workflow
When developing a feature (from `specs/` or a detailed prompt), you MUST:
1. Read `.claude/skills/testing/SKILL.md`.
2. For each layer touched, identify the specific tests to write.
3. Detail the test plan (class names, method names, key assertions) before writing test code.
4. Write tests following the established patterns for each layer.
5. Run all relevant suites and confirm all pass.

Skipping tests requires explicit user opt-out.

## Detailed docs
Read these when working in the relevant area:
- Backend patterns: `docs/architecture.md`
- Frontend architecture rules: `docs/frontend/architecture.md`
- Frontend conventions: `docs/frontend/coding-guidelines.md`
- API contracts: `docs/frontend/api-contracts.md`
- Testing: `docs/frontend/testing-strategy.md`

## Agent skills
Read these for decision-making guidance in specific domains:
- Database & migrations: `.claude/skills/database/SKILL.md`
- Backend API (CQRS/Clean Architecture): `.claude/skills/dotnet-api/SKILL.md`
- Testing strategy (full-stack): `.claude/skills/testing/SKILL.md`
- Discover & install new skills: `.claude/skills/find-skills/SKILL.md`

## Memory policy

After completing any task, update the project memory with interesting knowledge discovered:
- New patterns, conventions, or architectural decisions
- Key file locations and their purposes
- Important domain concepts or business rules

Follow the two-step process:
1. Write each memory to its own file in the memory directory (e.g., `project_foo.md`, `reference_patterns.md`) with frontmatter: `name`, `description`, `type` (`user`/`feedback`/`project`/`reference`). For `feedback` and `project` types, include a **Why:** and **How to apply:** line.
2. Add a pointer to `MEMORY.md` as an index entry — never write memory content directly into `MEMORY.md`.

Do not save: code patterns derivable from reading the code, git history, debugging recipes, or ephemeral task state.
