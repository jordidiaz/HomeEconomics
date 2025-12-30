# HomeEconomics

A personal finance management application built with .NET 9 and React TypeScript to help track income, expenses, and monthly budgets with recurring movement patterns.

## Features

- **Movement Management**: Create and manage income/expense items
- **Recurring Patterns**: Support for monthly, yearly, and custom frequency patterns
- **Monthly Budgeting**: Track actual vs planned movements per month
- **Payment Tracking**: Mark movements as paid/unpaid
- **Responsive UI**: Modern web interface built with React and SCSS

## Technology Stack

- **Backend**: .NET 10 ASP.NET Core Web API
- **Frontend**: React 16.13.1 with TypeScript
- **Database**: PostgreSQL with Entity Framework Core
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
- Adminer database admin interface on port 8080

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

The API will be available at `https://localhost:5001` (or `http://localhost:5000`)

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
cd src/HomeEconomics/spa
```

Install dependencies:

```bash
npm install
```

Start the development server:

```bash
npm start
```

The React app will be available at `http://localhost:3000`

#### Frontend Development Commands

```bash
# Start development server
npm start

# Build for production
npm run build

# Run tests
npm test

# Run tests in CI mode
npm run citest

# Lint TypeScript files
npm run lint

# Lint SCSS files
npm run stylelint

# Fix linting issues automatically
npm run lint:fix
```

## Project Structure

```
HomeEconomics/
├── src/
│   ├── Domain/                     # Domain entities and business logic
│   │   ├── Movements/             # Movement entities (income/expense)
│   │   └── MovementMonth/         # Monthly movement tracking
│   ├── Persistence/               # Data access layer
│   │   ├── Configurations/        # EF Core entity configurations
│   │   ├── Migrations/           # Database migrations
│   │   └── HomeEconomicsDbContext.cs
│   └── HomeEconomics/            # Web API application
│       ├── Features/             # Feature-based organization (CQRS)
│       │   ├── Movements/        # Movement CRUD operations
│       │   └── MovementMonths/   # Monthly movement operations
│       └── spa/                  # React frontend
│           ├── src/
│           │   ├── App/          # Main application components
│           │   ├── components/   # Reusable UI components
│           │   ├── styles/       # SCSS styling system
│           │   └── tests/        # Frontend tests
│           └── package.json
├── test/
│   ├── Domain.UnitTests/         # Domain layer unit tests
│   ├── HomeEconomics.UnitTests/  # Application unit tests
│   ├── HomeEconomics.IntegrationTests/ # API integration tests
├── Directory.Build.props         # Global MSBuild properties
├── HomeEconomics.sln            # Solution file
├── docker-compose.yaml          # Production Docker setup
└── docker-compose.development.yaml # Development environment
```

## Development Workflow

### Backend Development

1. **Domain Layer**: Create or modify entities in `src/Domain/`
2. **Database Changes**: Add migrations using Entity Framework
3. **API Features**: Implement CQRS commands/queries in `src/HomeEconomics/Features/`
4. **Testing**: Write unit tests for domain logic and integration tests for API endpoints

### Frontend Development

1. **Components**: Create reusable components in `src/HomeEconomics/spa/src/components/`
2. **Styling**: Use SCSS files following the established pattern
3. **API Integration**: Use Axios for HTTP requests to the backend API
4. **Testing**: Write Jest tests for components

### Running Tests

```bash
# Backend tests
dotnet test

# Frontend tests
cd src/HomeEconomics/spa && npm test
```

## Production Deployment

Build and run with Docker:

```bash
docker-compose up -d
```

This will:
- Build the .NET application
- Build the React frontend for production
- Start PostgreSQL database
- Serve the application on port 6001

## Database Migration (Heroku ↔ Local Docker)

### Export from Heroku and import into the local Docker container

1. Create and download a Heroku backup:

```bash
heroku pg:backups:capture --app <heroku-app-name>
heroku pg:backups:download --app <heroku-app-name> -o heroku.dump
```

2. Restore into the local PostgreSQL container:

```bash
docker compose up -d postgres
docker compose exec -T postgres pg_restore \
  --no-owner \
  --username=homeeconomics \
  --dbname=homeeconomics \
  --clean \
  --if-exists \
  < heroku.dump
```

### Export from the local Docker container and import into another container

1. Export a backup from the local container:

```bash
docker compose exec -T postgres pg_dump \
  --username=homeeconomics \
  --format=custom \
  --file=/tmp/homeeconomics.dump \
  homeeconomics
docker cp "$(docker compose ps -q postgres)":/tmp/homeeconomics.dump ./homeeconomics.dump
```

2. Import into another PostgreSQL container:

```bash
docker compose exec -T <other-postgres-service> pg_restore \
  --username=homeeconomics \
  --dbname=homeeconomics \
  --clean \
  --if-exists \
  < homeeconomics.dump
```

## Contributing

1. Create a feature branch
2. Make your changes
3. Run tests: `dotnet test` and `cd src/HomeEconomics/spa && npm test`
4. Run linting: `cd src/HomeEconomics/spa && npm run lint && npm run stylelint`
5. Submit a pull request

## Architecture

The application follows Clean Architecture principles:

- **Domain Layer**: Core business logic and entities
- **Persistence Layer**: Data access with Entity Framework Core
- **Application Layer**: API controllers and CQRS handlers using MediatR
- **Presentation Layer**: React TypeScript frontend

Key patterns used:
- CQRS with MediatR for command/query separation
- Repository pattern through Entity Framework DbContext
- Domain-Driven Design with rich domain models
