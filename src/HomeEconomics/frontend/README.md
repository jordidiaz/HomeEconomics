# HomeEconomics Frontend (Next.js)

## Project Purpose

This frontend is the Next.js (App Router) SPA foundation for HomeEconomics. It provides the baseline architecture, theme setup, and a minimal page used to validate the MUI configuration.

## Tech Stack

- Next.js (App Router)
- TypeScript (strict mode)
- MUI (Material UI)

## Requirements

- Node.js 18+

## Install

```bash
npm install
```

## Development

```bash
npm run dev
```

## Build

```bash
npm run build
```

## Testing

```bash
npm test
```

Includes unit, component, and integration tests (Vitest + React Testing Library).

Integration coverage includes `app/page.tsx` wiring for movement lifecycle reloads, add-to-current-month reload, month selector and paid toggle wiring, and month dialog success flows (edit amount, delete, move).

We use `@testing-library/jest-dom` for DOM matchers.

## Coverage

```bash
npm run test:coverage
```

## E2E Testing

End-to-end tests use Playwright and run against a local backend with the `homeeconomics-test` database.

### Prerequisites

Before running E2E tests, ensure:
- PostgreSQL is running
- Port 5050 (backend) and 3000 (frontend) are available
- .NET 10 SDK is installed

### Running E2E Tests

Install Playwright browser binaries once:

```bash
npx playwright install chromium
```

The E2E test runner automatically:
1. Starts the .NET backend with test database
2. Migrates the test database
3. Starts the Next.js dev server
4. Runs Playwright tests sequentially
5. Cleans up servers after tests complete

```bash
npm run e2e          # Run all E2E tests
npm run e2e:ui       # Run with Playwright UI
npm run e2e:headed   # Run with visible browser
npm run e2e:debug    # Debug mode
npm run e2e:report   # View last test report

# Run a single spec file
npx playwright test e2e/specs/movements-lifecycle.spec.ts
```

### Test Data Strategy

- Each test creates its own test data via API calls
- Tests use unique names to avoid collisions
- Cleanup happens in `afterEach` hooks
- Tests run sequentially to avoid race conditions

### E2E Test Coverage

The following user journeys are covered:

1. **Movements lifecycle** - create → edit → delete (confirm dialog)
2. **Month setup** - create current month if missing, verify list loads
3. **Add to current month** - from movements list, add → shows in month list
4. **Pay/unpay** - toggle paid state and verify list filter
5. **Edit amount** - dialog update, list reflects new amount
6. **Move to next month** - create next month, move, verify under next month
7. **Status form** - update account/cash on blur, verify balance and success

See `e2e-implementation-plan.md` for complete implementation details.

### Troubleshooting

**Backend fails to start:**
- Check if port 5050 is already in use: `lsof -i :5050`
- Verify PostgreSQL is running: `pg_isready`
- Check test database exists: `psql -U homeeconomics -l | grep homeeconomics-test`

**Frontend fails to start:**
- Check if port 3000 is in use: `lsof -i :3000`
- Try clearing Next.js cache: `rm -rf .next`

**Tests are flaky:**
- E2E tests run sequentially by design (workers: 1)
- Check network requests in Playwright report: `npm run e2e:report`
- Use `npm run e2e:debug` to step through tests
