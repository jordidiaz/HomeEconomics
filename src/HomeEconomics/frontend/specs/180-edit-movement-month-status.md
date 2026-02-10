# Spec 180 – Edit Movement Month Status

## Goal

Allow the user to edit the movement month status amounts and persist changes via the status endpoint.

---

## Scope

This spec applies to the MovementMonthStatusForm rendered above the month selector.

---

## API Contract

POST /api/movement-months/{movementMonthId:int}/add-status

See `api-contracts.md` – Movement Months / Add status

---

## UI Behavior

- The **accountAmount** and **cashAmount** inputs must be editable (not read-only)
- When either input value changes:
  - Call the add-status endpoint with the current values only when the inpiut loses focus
- The **Balance** label must be recalculated based on the updated values

---

## Labels

- accountAmount: **"Dinero en cuenta"**
- cashAmount: **"Dinero en cash"**
- Balance: **"Balance"**

All user-facing text must be in Spanish.

---

## Interaction Flow

1. User edits account or cash input
2. After debounce delay, the status API is called
3. On success:
   - The form keeps the edited values
   - The Balance is recalculated

---

## Validation & Errors

- Inputs must accept numeric values only
- If the API request fails:
  - A generic error message must be displayed near the form
  - Values remain as entered

---

## States

- While submitting:
  - Inputs must be disabled

---

## Technical Constraints

- Follow `architecture.md` and `coding-guidelines.md`
- API access via a dedicated service
- Update logic must be handled via a hook
- Reuse existing data-fetching mechanisms when needed
- No new dependencies

---

## Out of Scope

- Manual save button
- Optimistic updates beyond the debounce submit
- Additional status fields
