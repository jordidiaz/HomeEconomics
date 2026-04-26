# HomeEconomics

A personal finance management application built with .NET 10 and Next.js to help track income, expenses, and monthly budgets with recurring movement patterns.

## Features

- **Movement Management**: Create and manage income/expense items
- **Recurring Patterns**: Support for monthly, yearly, and custom frequency patterns
- **Monthly Budgeting**: Track actual vs planned movements per month
- **Payment Tracking**: Mark movements as paid/unpaid
- **Responsive UI**: Modern web interface built with Next.js and MUI

## Technology Stack

- **Backend**: .NET 10 ASP.NET Core Web API
- **Frontend**: Next.js 14 (App Router) with TypeScript and MUI
- **Database**: PostgreSQL with Entity Framework Core
- **Testing**: xUnit + FluentAssertions (backend), Vitest + RTL + Playwright (frontend)
- **Architecture**: Clean Architecture with CQRS powered by LiteBus

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Node.js 14+](https://nodejs.org/)
- [PostgreSQL](https://www.postgresql.org/) (or Docker for development)

## Getting Started

### 1. Clone the Repository

```bash
git clone <repository-url>
cd HomeEconomics
```

### 2. Development Environment Setup

#### Option A: Using Docker (Recommended for Development)

Start the development database:

```bash
docker-compose -f docker-compose.development.yaml up -d
```

This provides:
- PostgreSQL database on port 5432

### 3. Backend Development

Navigate to the project root and restore dependencies:

```bash
dotnet restore
```

Apply database migrations:

```bash
dotnet ef database update --project src/Persistence --startup-project src/HomeEconomics
```

Run the API:

```bash
dotnet run --project src/HomeEconomics
```

The API will be available at `http://localhost:5050`

#### Backend Development Commands

```bash
# Build the solution
dotnet build

# Run all tests
dotnet test

# Run specific test project
dotnet test test/Domain.UnitTests
dotnet test test/HomeEconomics.IntegrationTests

# Create new migration
dotnet ef migrations add <MigrationName> --project src/Persistence --startup-project src/HomeEconomics

# Update database to latest migration
dotnet ef database update --project src/Persistence --startup-project src/HomeEconomics
```

### 4. Frontend Development

Navigate to the frontend directory:

```bash
cd src/HomeEconomics/frontend
```

Install dependencies:

```bash
npm install
```

Start the development server:

```bash
npm run dev
```

The Next.js app will be available at `http://localhost:3000`

#### Frontend Development Commands

```bash
# Start development server
npm run dev

# Build for production
npm run build

# Run tests (watch mode)
npm test

# Run tests in CI mode
npm run test:ci

# Run E2E tests (requires local backend running)
npm run e2e

# Run E2E tests with Playwright UI
npm run e2e:ui
```

## Project Structure

```
HomeEconomics/
├── src/
│   ├── Domain/                          # Domain entities and business logic
│   │   ├── Movements/                  # Movement entities (income/expense)
│   │   └── MovementMonth/              # Monthly movement tracking
│   ├── Persistence/                     # Data access layer
│   │   ├── Configurations/             # EF Core entity configurations
│   │   ├── Extensions/                 # Service registration helpers
│   │   ├── Migrations/                 # Database migrations
│   │   └── HomeEconomicsDbContext.cs
│   └── HomeEconomics/                   # Web API application
│       ├── Features/                   # Feature-based organization (CQRS)
│       │   ├── Movements/              # Movement CRUD operations
│       │   └── MovementMonths/         # Monthly movement operations
│       ├── Extensions/                 # Middleware and service extensions
│       ├── Filters/                    # Action filters (model validation)
│       ├── Services/                   # Application services
│       └── frontend/                   # Next.js 14 App Router frontend
│           ├── app/                    # Routes and layouts
│           ├── components/             # Presentational UI components (MUI)
│           ├── hooks/                  # State and side-effect hooks
│           ├── services/               # API access layer (typed fetch)
│           ├── types/                  # Shared TypeScript types
│           └── e2e/                    # Playwright end-to-end tests
├── test/
│   ├── Domain.UnitTests/               # Domain layer unit tests
│   ├── HomeEconomics.UnitTests/        # Application unit tests (handlers)
│   └── HomeEconomics.IntegrationTests/ # API integration tests
├── docs/
│   ├── architecture.md                 # Backend architecture and patterns
│   ├── api.md                          # API reference
│   └── frontend/                       # Frontend detailed docs
├── specs/                              # Feature specifications
├── Directory.Build.props               # Global MSBuild properties
├── HomeEconomics.sln                   # Solution file
├── docker-compose.yaml                 # Production Docker setup
└── docker-compose.development.yaml     # Development environment (Postgres)
```

## Development Workflow

### Backend Development

1. **Domain Layer**: Create or modify entities in `src/Domain/`
2. **Database Changes**: Add migrations using Entity Framework
3. **API Features**: Implement CQRS commands/queries in `src/HomeEconomics/Features/`
4. **Testing**: Write unit tests for domain logic and integration tests for API endpoints

### Frontend Development

1. **Components**: Create reusable components in `src/HomeEconomics/frontend/components/`
2. **Styling**: Use MUI theme tokens and the `sx` prop
3. **API Integration**: Use typed fetch-based services in `frontend/services/`
4. **Testing**: Write Vitest tests for components and hooks

### Running Tests

```bash
# Backend tests
dotnet test

# Frontend unit/component tests
cd src/HomeEconomics/frontend && npm run test:ci

# Frontend E2E tests (requires local backend running)
cd src/HomeEconomics/frontend && npm run e2e
```

## Production Deployment

The Docker image is built automatically on every merge to `main` and published to GitHub Container Registry (GHCR) at `ghcr.io/jordidiaz/homeeconomics`. The image is private — a Personal Access Token (PAT) is required to pull it.

### First-time setup

1. Copy the environment template and fill in your Supabase credentials:

```bash
cp .env.example .env
# Edit .env with your real Supabase connection string
```

2. Authenticate Docker to GHCR (one-time per host):

```bash
echo "$GHCR_TOKEN" | docker login ghcr.io -u jordidiaz --password-stdin
```

`GHCR_TOKEN` is a **classic Personal Access Token** with the `read:packages` scope — generate one at GitHub → Settings → Developer settings → Personal access tokens.

3. Start the app:

```bash
docker compose up -d
```

The application will be available on port 6001.

### Updating the running app

After every merge to `main`, CI publishes a new image tagged `:latest` and a short-SHA tag (e.g. `:sha-a1b2c3d`). To update a running instance:

```bash
# Pull the new image and restart in one step
docker compose pull && docker compose up -d

# Confirm the container is running the new image
docker compose ps
docker image ls ghcr.io/jordidiaz/homeeconomics
```

To pin to a specific build instead of `:latest`, edit `docker-compose.yaml` and replace `latest` with the desired `sha-<short>` tag (visible on the [Releases](../../releases) page).

> **Local rebuild**: contributors who want to test local changes without publishing can still build manually with `docker compose build` — the `Dockerfile` at `src/HomeEconomics/Dockerfile` is unchanged.

## Updating Dependencies

This project uses Claude Code skills to update dependencies safely. Each skill discovers outdated packages, classifies them by semver risk, auto-applies safe updates, and verifies the build and tests.

### Backend (NuGet)

```bash
# Using the skill (slash command)
/update-backend-deps

# Or ask the agent directly
"update backend dependencies"
```

### Frontend (npm)

```bash
# Using the skill (slash command)
/update-frontend-deps

# Or ask the agent directly
"update frontend dependencies"
```

### Both layers at once

```bash
# Ask the agent to handle everything
"update dependencies"
```

MINOR and PATCH updates are applied automatically. MAJOR updates require explicit approval before proceeding.

## Contributing

1. Create a feature branch from `main`
2. Make your changes
3. Run backend tests: `dotnet test`
4. Run frontend tests: `cd src/HomeEconomics/frontend && npm run test:ci`
5. Submit a pull request — CI will run the full test suite automatically
6. Once all checks pass, merge to `main`; the release workflow publishes a new Docker image

## Architecture

The application follows Clean Architecture principles:

- **Domain Layer**: Core business logic and entities
- **Persistence Layer**: Data access with Entity Framework Core
- **Application Layer**: API controllers and CQRS handlers using LiteBus
- **Presentation Layer**: Next.js TypeScript frontend

Key patterns used:
- CQRS with LiteBus for command/query separation
- Repository pattern through Entity Framework DbContext
- Domain-Driven Design with rich domain models
