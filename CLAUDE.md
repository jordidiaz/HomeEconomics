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

## Detailed docs
Read these when working in the relevant area:
- Backend patterns: `docs/architecture.md`
- Frontend architecture rules: `docs/frontend/architecture.md`
- Frontend conventions: `docs/frontend/coding-guidelines.md`
- API contracts: `docs/frontend/api-contracts.md`
- Testing: `docs/frontend/testing-strategy.md`

## Agent skills
Read these for decision-making guidance in specific domains:
- Database & migrations: `.agents/skills/database/SKILL.md`
- Backend API (CQRS/Clean Architecture): `.agents/skills/dotnet-api/SKILL.md`
- Testing strategy: `.agents/skills/dotnet-testing/SKILL.md`
- Discover & install new skills: `.agents/skills/find-skills/SKILL.md`
