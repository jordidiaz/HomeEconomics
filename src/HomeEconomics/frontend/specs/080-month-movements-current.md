# Spec 080 – Current Month Movements List

## Goal

Display the list of movements for the current month so the user can review the actual monthly financial activity.

---

## API Contract

GET /api/movement-months/{year:int}/{month:int}

See `api-contracts.md` – Movement Months / GET /api/movement-months/{year}/{month}

---

## Date Resolution

- The current year and month must be calculated at runtime
- Month value must be computed as an integer between **1 and 12**
- Year must be the full calendar year (e.g. 2026)

The resolved `(year, month)` must be used to call the API.

---

## UI Requirements

- Display a list of **MonthMovements** for the current month
- For each MonthMovement, the following information must be shown:
  - Name
  - Amount
  - Type (income or expense)
  - Paid (true or false)

- MonthMovements must be visually distinguishable by type (income vs expense),
  following the same visual convention used in the Movements list
- MonthMovements must be visually distinguishable by paid (true vs false)

---

## Placement

- The current month movements list must be displayed in the **left section** of the split layout
- The right section must remain unchanged

---

## States

- Loading state while fetching data
- Error state if the request fails
- Empty state if no month movements exist for the current month

---

## Behavior

- The list is read-only
- No actions (edit, delete, pay, unpay) are required in this version
- Data must be fetched once on page load

---

## Technical Constraints

- Follow `architecture.md` and `coding-guidelines.md`
- API access via a dedicated service
- Data fetching and state management via a hook
- Date calculation logic must not be duplicated across components
- No new dependencies

---

## Out of Scope

- Navigation between months
- Creating or modifying month movements
- Payment status visualization
- Totals or aggregates
- Caching or polling
