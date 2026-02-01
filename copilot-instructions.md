# HomeEconomics AI Coding Instructions

## Framework & Language Standards

- Target: .NET 10 (configured in [Directory.Build.props](Directory.Build.props))
- C# 14.0 with nullable reference types enabled
- `TreatWarningsAsErrors` is enabled - all warnings must be resolved
- Use implicit usings

## Architecture Overview

**Clean Architecture + DDD + CQRS** with vertical slice organization:

- [src/Domain/](src/Domain/) - Rich domain models (aggregate roots: `Movement`, `MovementMonth`)
- [src/Persistence/](src/Persistence/) - EF Core with code-first migrations, entity configurations in [Configurations/](src/Persistence/Configurations/)
- [src/HomeEconomics/Features/](src/HomeEconomics/Features/) - Vertical slices organized by feature (not technical layer)

### CQRS Pattern with LiteBus

Each feature uses the mediator pattern via LiteBus:
- Commands: `ICommand<TResult>` handled by `ICommandHandler<TCommand, TResult>`
- Queries: `IQuery<TResult>` handled by `IQueryHandler<TQuery, TResult>`
- Controllers inject `ICommandMediator` and `IQueryMediator`, never call handlers directly

Example structure from [Features/Movements/Create.cs](src/HomeEconomics/Features/Movements/Create.cs):
```csharp
public class Create
{
    public record Command(...) : ICommand<int>;
    public class Validator : AbstractValidator<Command> { }
    [UsedImplicitly] public class Handler(...) : ICommandHandler<Command, int> { }
}
```

### Domain Model Conventions

- Aggregate roots implement `IAggregateRoot` marker interface
- Entities inherit from `Entity` base class (provides `Id`)
- Domain logic stays in domain models - no anemic models
- Use private setters, expose behavior through methods (e.g., `Movement.SetMonthlyFrequency()`)
- Value objects like `Frequency` are owned entities configured in [Persistence/Configurations/](src/Persistence/Configurations/)

## Testing Requirements

**Mandatory for behavior changes.** Three test levels:

1. **Unit Tests** ([test/Domain.UnitTests/](test/Domain.UnitTests/)) - Domain logic with FluentAssertions
2. **Integration Tests** ([test/HomeEconomics.IntegrationTests/](test/HomeEconomics.IntegrationTests/)) - API endpoints with test database
   - Inherit from `IntegrationTestBase` which uses Respawn to reset database between tests
   - Use `Fixture.SendCommandToMediatorAsync()` and `Fixture.QueryDbContextAsync()` helpers
3. **Functional Tests** ([test/HomeEconomics.FunctionalTests/](test/HomeEconomics.FunctionalTests/)) - End-to-end scenarios

Run tests: `dotnet test` (all) or `dotnet test test/<project-name>`

## Database & Migrations

- PostgreSQL with EF Core
- Development environment: `docker-compose -f docker-compose.development.yaml up`
- Create migration: `dotnet ef migrations add <Name> --project src/Persistence --startup-project src/HomeEconomics`
- Apply migrations: `dotnet ef database update --project src/Persistence --startup-project src/HomeEconomics`
- Entity configurations must be in [src/Persistence/Configurations/](src/Persistence/Configurations/) and implement `IEntityTypeConfiguration<T>`

## Validation & Error Handling

- Use FluentValidation for all command/query validation
- `ValidateModelAttribute` filter auto-validates and returns 400 for validation errors
- Map domain exceptions in [ServiceCollectionExtensions.cs](src/HomeEconomics/Extensions/ServiceCollectionExtensions.cs) using `ProblemDetails` middleware
- `InvalidOperationException` → 409 Conflict (configured globally)

## Development Workflow

- Build: `dotnet build`
- Run API: `dotnet run --project src/HomeEconomics` (available at https://localhost:5001)
- Frontend dev: `cd src/HomeEconomics/spa && npm start` (port 3000)
- All warnings treated as errors - must be fixed before committing

## Critical Rules

- **Do not change public contracts** without approval
- **Do not refactor unrelated code** - keep changes focused
- **Tests are mandatory** for any behavior change
- Use `[UsedImplicitly]` attribute on handlers for ReSharper
- Follow existing feature structure: one file per command/query with nested types

This repository defines AI skills under `.ai/skills`.

When performing changes:
- Identify the relevant skill
- Follow its decision rules
- Do not invent patterns outside those skills
