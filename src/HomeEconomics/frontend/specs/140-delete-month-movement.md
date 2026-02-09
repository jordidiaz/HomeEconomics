# Spec 140 – Delete Month Movement

## Goal

Allow the user to delete a month movement from the current month movements list using a confirmation dialog.

---

## Scope

This spec applies only to **MonthMovements** displayed in the current month movements list.

---

## API Contract

DELETE /api/movement-months/{movementMonthId:int}/month-movements/{monthMovementId:int}

See `api-contracts.md` – Movement Months / Delete a month movement

---

## UI Placement

- Each month movement item must expose an **actions area**
- The delete action must be displayed:
  - Immediately **below the amount** of the month movement
  - **Between** the edit action and the pay/unpay action

---

## Dialog Content

The delete confirmation dialog must reuse the **schema and copy style** from the movement deletion dialog (see `confirm-delete-movement-dialog.tsx`).

The dialog must include:

- Title: **"Confirmar borrado"**
- Descriptive text explaining the action is irreversible and includes the movement name
- Primary action to confirm deletion
- Secondary action to cancel

All user-facing text must be in Spanish.

---

## Interaction Flow

1. User clicks the delete action on a month movement
2. Confirmation dialog is displayed
3. User chooses:
   - **Confirm** → deletion is executed
   - **Cancel** → dialog is closed, no action is taken

---

## Behavior on Confirm

- The delete request is sent to the API
- While deleting:
  - The confirm action must be disabled
  - The delete action for that movement must be disabled
- On success:
  - The dialog is closed
  - The month movements list is reloaded

---

## States

- Loading or error state must not affect other movement items

---

## Error Handling

- If deletion fails:
  - The dialog must remain open
  - A generic error message must be displayed within the dialog
  - The movement remains unchanged

---

## Technical Constraints

- Follow `architecture.md` and `coding-guidelines.md`
- API access via a dedicated service
- Delete logic must be handled via a hook
- Reload logic must reuse existing data-fetching mechanisms
- No new dependencies

---

## Out of Scope

- Optimistic updates
- Batch delete
- Inline delete without confirmation
- Custom animations
