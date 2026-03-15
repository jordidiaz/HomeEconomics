# Frontend – Claude Code Context

Next.js 14 App Router frontend for HomeEconomics. Run commands from this directory.

## Commands
```
npm run dev          # dev server
npm run build        # production build
npm test             # Vitest (watch)
npm run test:ci      # Vitest (single run, for CI)
npm run e2e          # Playwright E2E (requires local backend running)
```

## Mandatory architecture rules
Data flows one way: `app/ → hooks/ → services/ → Backend API`. Reverse is forbidden.

| Layer | Responsibility | Forbidden |
|---|---|---|
| `app/` | Routes, layouts, wiring hooks to UI | fetch calls, business logic |
| `hooks/` | Loading/error state, data transformation | JSX, direct fetch |
| `services/` | All fetch calls, typed DTOs | React imports, state |
| `components/` | Presentational UI, props only | API calls, side effects |

## Key conventions
- TypeScript `strict: true`, no `any`
- File names `kebab-case`, components `PascalCase`, hooks `useX`
- MUI only for UI — `sx` prop and theme tokens, no inline styles, no CSS files
- Services throw on non-2xx; hooks map errors to UI states; errors must be user-visible
- No `console.log` in committed code
- User-facing text in Spanish

## Detailed docs (read these before implementing)
Read in this order; earlier docs take precedence over later ones:
1. `docs/frontend/architecture.md` — mandatory rules, overrides everything
2. `docs/frontend/coding-guidelines.md` — conventions, overrides specs
3. `docs/frontend/api-contracts.md` — API behavioral contracts
4. `docs/frontend/testing-strategy.md` — test layers and coverage targets
