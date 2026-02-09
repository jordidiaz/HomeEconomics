# Spec 150 – Move Month Movement to Next Month

## Goal

Allow the user to move a month movement to the next movement month using a confirmation dialog.

---

## Scope

This spec applies only to **MonthMovements** displayed in the current month movements list.

---

## API Contract

POST /api/movement-months/{movementMonthId:int}/month-movements/{monthMovementId:int}/to-next-movement-month

See `api-contracts.md` – Movement Months / Move movement to the next month

---

## UI Placement

- Each month movement item must expose an **actions area**
- The move action must be displayed:
  - Immediately **below the amount** of the month movement
  - **Before** the edit action

---

## Visibility

- The move action must be displayed **only if** the next movement month exists

---

## Dialog Content

The confirmation dialog must reuse the **schema and copy style** from the movement deletion dialog (see `confirm-delete-movement-dialog.tsx`).

The dialog must include:

- Title: **"Confirmar acción"**
- Descriptive text explaining the action and includes the movement name
- Primary action to confirm
- Secondary action to cancel

All user-facing text must be in Spanish.

---

## Interaction Flow

1. User clicks the move action on a month movement
2. Confirmation dialog is displayed
3. User chooses:
   - **Confirm** → action is executed
   - **Cancel** → dialog is closed, no action is taken

---

## Behavior on Confirm

- The move request is sent to the API
- While processing:
  - The confirm action must be disabled
  - The move action for that movement must be disabled
- On success:
  - The dialog is closed
  - The month movements list is reloaded

---

## States

- Loading or error state must not affect other movement items

---

## Error Handling

- If the API request fails:
  - The dialog must remain open
  - A generic error message must be displayed within the dialog
  - The movement remains unchanged

---

## Technical Constraints

- Follow `architecture.md` and `coding-guidelines.md`
- API access via a dedicated service
- Move logic must be handled via a hook
- Reload logic must reuse existing data-fetching mechanisms
- No new dependencies

---

## Out of Scope

- Optimistic updates
- Batch move
- Inline move without confirmation
- Custom animations
