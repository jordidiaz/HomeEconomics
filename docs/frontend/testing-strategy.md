# Frontend Testing Strategy (Vitest + RTL + Playwright)

This document captures the complete testing strategy for the Next.js frontend in
`src/HomeEconomics/frontend`. It reflects the current architecture, coding
guidelines, and feature specs.

## Recommendation for E2E Backend

Use a real backend locally for E2E (not mocked). This validates API contract
behavior and state transitions (pay/unpay, move, status updates). For stability,
run E2E against a dedicated local test database or a resettable environment, and
create test data via API in each test or in `beforeEach`.

## 1) Test Layers and Scope

- Unit tests (Vitest)
  - Services: request/response, error handling, caching.
  - Hooks: validation, state transitions, derived data, error mapping.
- Component tests (React Testing Library)
  - Presentational components and dialogs for UI behavior and states.
- Integration tests (RTL + Vitest)
  - `app/page.tsx` wiring with mocked hooks/services.
- E2E tests (Playwright)
  - Critical user journeys against local backend.

## 2) Test Conventions

- No snapshot tests.
- Test behavior, not implementation details.
- Mock services, not hooks.
- Assert user-visible Spanish copy where applicable.
- Mock `Date` for month selector logic.

## 3) Unit Tests

### Services

- `services/movements-service.ts`
  - `getAll` returns data, errors on non-2xx, and caches in-flight request.
  - `create`, `update`, `delete` send correct method/body.
- `services/movement-months-service.ts`
  - All endpoints: correct URL/method/body.
  - Non-2xx throws include status.
  - `getByYearMonth` caches by key.

### Hooks

- `hooks/use-movements.ts`
  - Loading -> data, error state, label formatting for type/frequency.
- `hooks/use-movement-form.ts`
  - Required validation, amount > 0, frequency rules, edit mode, reset, submit error.
- `hooks/use-add-month-movement-form.ts`
  - Validation + error display on missing month.
- `hooks/use-delete-movement.ts`
  - Clear error, success path, failure path.
- `hooks/use-add-movement-to-current-month.ts`
  - Per-item loading/error.
- `hooks/use-month-movement-selector.ts`
  - Current/next month resolution, 404 handling, create month flow.
- `hooks/use-current-month-movements.ts`
  - Paid filter behavior, action state isolation, error messages.
- `hooks/use-movement-month-status-form.ts`
  - Balance calc, numeric validation, dedupe submit, error message.

## 4) Component Tests (RTL)

- Forms: `components/movement-form.tsx`, `components/add-month-movement-form.tsx`
  - Required inputs, submit/cancel, disabled while submitting, Spanish labels.
- Lists: `components/movements-list.tsx`, `components/current-month-movements-list.tsx`
  - Action buttons enable/disable logic, paid indicators, error messages.
- Dialogs:
  - `components/confirm-delete-movement-dialog.tsx`
  - `components/confirm-delete-month-movement-dialog.tsx`
  - `components/edit-month-movement-amount-dialog.tsx`
  - `components/confirm-move-month-movement-dialog.tsx`
  - Open/close, disabled while submitting, error text.
- Selector: `components/month-movement-month-selector.tsx`
  - Month toggles, create month buttons, error display.

## 5) Integration Tests (RTL + Vitest)

`app/page.tsx`:

- Movement create/edit/delete wiring triggers reload.
- Add-to-current-month action triggers month reload.
- Selector switching reloads the list.
- Paid toggle shows correct list.
- Dialog flows (edit amount, delete, move) call hook actions and close on success.

### Integration coverage matrix

| Area | Scenario | Status | Test file |
| --- | --- | --- | --- |
| Movements wiring | Create/edit/delete triggers movements reload | Implemented | `app/__tests__/page.test.tsx` |
| Month wiring | Add-to-current-month triggers month reload | Implemented | `app/__tests__/page.test.tsx` |
| Selector wiring | Switching current/next month is wired to selector state/actions | Implemented | `app/__tests__/page.test.tsx` |
| Paid filter wiring | Paid toggle is wired to hook state update | Implemented | `app/__tests__/page.test.tsx` |
| Dialog success flow | Edit amount action + close on success | Implemented | `app/__tests__/page.test.tsx` |
| Dialog success flow | Delete month movement action + close on success | Implemented | `app/__tests__/page.test.tsx` |
| Dialog success flow | Move to next month action + close on success | Implemented | `app/__tests__/page.test.tsx` |

## 6) E2E Tests (Playwright, local backend)

Core journeys (create data via API in each test):

1) Movements lifecycle: create -> edit -> delete (confirm dialog).
2) Month setup: create current month if missing, verify list loads.
3) Add to current month: from movements list, add -> shows in month list.
4) Pay/unpay: toggle paid state and verify list filter.
5) Edit amount: dialog update, list reflects new amount.
6) Move to next month: create next month, move, verify under next month.
7) Status form: update account/cash on blur, verify balance and success.

## 7) Test Data Strategy

- Create all data via API in `beforeEach` or per test.
- Keep test IDs isolated to avoid collisions.
- Optional cleanup via API if needed.

## 8) Coverage Targets

- Global: 70-80%.
- Hooks/Services: aim 80%+ within their modules.

## 9) Local-Only Commands (planned)

- Unit + component + integration: `npm test` (once scripts are added).
- E2E: `npx playwright test`.

## Agent skills
- For test layer and scope decisions: `.claude/skills/testing/SKILL.md`
