# Spec 010 – Movements List

## Goal

Display a list of movements so the user can review existing recurring and one-off movements.

---

## API Contract

GET /api/movements

See `api-contracts.md` – Movements / GET /api/movements

---

## UI Requirements

- Display a list of movements
- Each movement must show:
  - Name / description
  - Amount
  - Type (income or expense)
  - Frequency
- Movements must be visually distinguishable by type (income vs expense)

---

## Interaction

- No inline editing
- No actions (edit/delete) in this version
- Read-only view

---

## States

- Loading state while fetching data
- Error state if the request fails
- Empty state if no movements exist

---

## Technical Constraints

- Follow `architecture.md` and `coding-guidelines.md`
- API access via a dedicated service
- Data fetching and state management via a hook
- No new dependencies

---

## Out of Scope

- Pagination
- Filtering
- Sorting
- Editing or deleting movements
- Navigation to detail view
