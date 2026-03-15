# Architecture

## Overview
HomeEconomics follows **Clean Architecture** with a clear separation of responsibilities between domain, persistence, and the web application. The API uses **CQRS** with LiteBus to separate commands (mutations) from queries (reads), and exposes endpoints through minimal controllers in `Features/*`.

The frontend (Next.js App Router) lives under `src/HomeEconomics/frontend` and consumes the ASP.NET Core API.

## Folder structure

```
src/
├── Domain/                 # Domain model and business rules
├── Persistence/            # Data infrastructure (EF Core)
└── HomeEconomics/          # Web API application + frontend
    ├── Features/           # CQRS by feature (command/query/handler/validator)
    ├── Extensions/         # Service registration and middleware
    ├── Filters/            # MVC filters (validation)
    ├── Services/           # Application services
    └── frontend/           # Next.js App Router frontend
```

## Layers and responsibilities

### Domain (`src/Domain`)
- **Entities and aggregates**: `Movement`, `MovementMonth`, `MonthMovement`, `Status`, etc.
- **Business rules**: encapsulated in domain methods (`SetMonthlyFrequency`, `AddMonthMovement`, etc.).
- **Common base**: `Entity` and `IAggregateRoot` define basic conventions.

### Persistence (`src/Persistence`)
- **EF Core**: `HomeEconomicsDbContext` manages `DbSet`s for movements and months.
- **Configurations**: `Configurations/` centralizes entity mapping.
- **Data access**: extension methods (for example `GetMovementAsync`) encapsulate repeated queries.

### Application/API (`src/HomeEconomics`)
- **Endpoints**: controllers in `Features/*/*Controller.cs`.
- **CQRS**:
  - *Commands* and *Queries* with their `Handler` in the same feature file.
  - LiteBus configures modules in `AddHomeEconomicsMediator()`.
- **Validation**: FluentValidation applied by the `ValidateModelAttribute` filter.
- **Errors**: `ProblemDetails` middleware standardizes error responses.
- **Swagger**: documents and tests the API (`/swagger`).
- **Health checks**: `/self` and `/npgsql`.

### Frontend (`src/HomeEconomics/frontend`)
- **Next.js App Router** with TypeScript strict mode and MUI.
- **Data flow**: `app/ → hooks/ → services/ → API` (one-way, enforced).
- **Services** (`services/`) encapsulate all fetch calls; hooks own loading/error state; components are presentational.
- See `docs/frontend/architecture.md` for full rules.

## Patterns and conventions

- **Feature folders (CQRS)**: each feature file groups `Command/Query`, `Validator`, and `Handler`.
- **Edge validation**: invalid inputs are rejected; the filter adds errors to `ModelState` and returns 400.
- **Business errors**: controlled exceptions (e.g., `InvalidOperationException`) are translated to 409.
- **DTOs and responses**: generated at the feature level (for example, `MovementMonthResponse`).
- **Enums serialized as int**: `MovementType`, `FrequencyType`, and `Month` are sent as integers.

## Configuration conventions

- **Swagger**: `swagger/hm/swagger.json` and UI at `/swagger`.
- **CORS**: enabled only in development.
- **Frontend**: Next.js serves its own dev server; API proxied via `next.config.js` in development.
- **API base URL**: `/api/` prefix.

## Common extension points

- **New endpoint**: create a file in `Features/<Feature>/` with command/query and add a method in the corresponding controller.
- **New business case**: extend an entity in `Domain/` and update mapping/configuration if needed.
- **New validation**: add a `Validator` in the feature; the filter detects it automatically.

## Agent skills
- For layering and CQRS decisions: `.agents/skills/dotnet-api/SKILL.md`
- For schema, migration, and persistence decisions: `.agents/skills/database/SKILL.md`
