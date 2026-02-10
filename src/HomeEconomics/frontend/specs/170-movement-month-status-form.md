# Spec 170 – Movement Month Status Form

## Goal

Display the current movement month status data above the month selector.

---

## Scope

This spec applies to the current month selection view that already requests movement month data.

---

## API Contract

GET /api/movement-months/{year:int}/{month:int}

See `api-contracts.md` – Movement Months / Get movement month

---

## UI Placement

- A new form must be displayed **above** the month selector
- The form must be visually grouped with the selector

---

## Data Rendering

Use the `status` object from the movement month response and render:

- `accountAmount` in a text input
- `cashAmount` in a text input
- The calculation `(accountAmount + cashAmount) - (pendingTotalExpenses - pendingTotalIncomes)` in a label just below

---

## Labels

- `accountAmount`: **"Dinero en cuenta"**
- `cashAmount`: **"Dinero en cash"**
- `(accountAmount + cashAmount) - (pendingTotalExpenses - pendingTotalIncomes)`: **"Balance"**

All user-facing text must be in Spanish.

---

## Interaction

- This form is display-only (no submission or API calls)
- Inputs are read-only and reflect the current movement month status

---

## States

- While loading:
  - The form must be disabled or display a loading state
- If data is unavailable:
  - The form is not displayed

---

## Technical Constraints

- Follow `architecture.md` and `coding-guidelines.md`
- Reuse existing data-fetching mechanisms
- No new dependencies

---

## Out of Scope

- Editing or saving status values
- Additional derived metrics
- Custom formatting beyond existing locale conventions
