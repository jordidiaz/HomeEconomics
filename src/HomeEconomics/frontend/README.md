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
