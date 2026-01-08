# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

HomeEconomics is a personal finance management application built with .NET 9 backend API and React TypeScript frontend. It helps users track income and expenses with recurring movement patterns and monthly budgeting features.

## Technology Stack

**Backend**: .NET 9 ASP.NET Core Web API, PostgreSQL with Entity Framework Core, LiteBus for CQRS pattern
**Frontend**: React 16.13.1 with TypeScript, SCSS, Create React App
**Database**: PostgreSQL with code-first migrations
**Architecture**: Clean Architecture with Domain-Driven Design principles

## Essential Commands

### Backend Development
```bash
# Build solution
dotnet build

# Run application 
dotnet run --project src/HomeEconomics

# Run all tests
dotnet test

# Database migrations
dotnet ef migrations add <MigrationName> --project src/Persistence --startup-project src/HomeEconomics
dotnet ef database update --project src/Persistence --startup-project src/HomeEconomics

# Development environment with database
docker-compose -f docker-compose.development.yaml up
```

### Frontend Development
```bash
cd src/HomeEconomics/spa

# Install dependencies
npm install

# Start development server
npm start

# Build for production
npm run build

# Run tests
npm test

# Linting
npm run lint        # ESLint for TypeScript
npm run stylelint   # Stylelint for SCSS
```

## Architecture Overview

### Project Structure
- `src/Domain/` - Domain entities and business logic (Movement, MovementMonth)
- `src/Persistence/` - Data access layer with EF Core configurations
- `src/HomeEconomics/` - Web API application layer
- `src/HomeEconomics/Features/` - CQRS feature organization using LiteBus
- `src/HomeEconomics/spa/` - React TypeScript frontend
- `test/` - Comprehensive test suite (Unit, Integration, Functional)

### Key Patterns
- **Clean Architecture** with clear layer separation
- **CQRS with LiteBus** for command/query separation
- **Feature-based organization** rather than technical layering
- **Domain-Driven Design** with rich domain models

### Core Domain Concepts
- **Movement**: Income/expense items with recurring patterns (None, Monthly, Yearly, Custom frequency)
- **MovementMonth**: Actual movement tracking for specific months with payment status
- **MonthCollection**: Domain wrapper for managing monthly patterns

## Database Configuration

- Environment variable: `HOMEECONOMICS_CONNECTION_STRING` (PostgreSQL connection string)
- Development database available via `docker-compose.development.yaml`
- Migrations use code-first approach with Entity Framework Core

## Testing Strategy

- **Unit Tests**: Domain logic and components (xUnit + FluentAssertions)
- **Integration Tests**: API endpoints with test database
- **Functional Tests**: End-to-end scenarios
- **Frontend Tests**: React components with Jest

## Development Environment

- **API Port**: 6001 (production), standard ASP.NET Core ports (development)
- **React Dev Server**: 3000
- **Database**: 5432 (PostgreSQL)
- **Adminer**: 8080 (development database admin)

## Code Quality Standards

- Warnings treated as errors
- Nullable reference types enabled
- FluentValidation for request validation
- Comprehensive ESLint and Stylelint configuration
- Pre-commit hooks with Husky and lint-staged