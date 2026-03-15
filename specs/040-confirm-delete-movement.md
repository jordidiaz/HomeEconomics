# Spec 040 – Confirm Delete Movement

## Goal

Prevent accidental deletion of movements by introducing a confirmation dialog before deleting a movement.

---

## API Contract

DELETE /api/movements/{id:int}

See `api-contracts.md` – Movements / DELETE /api/movements/{id}

---

## UI Requirements

- Triggering the delete action must open a confirmation dialog
- The dialog must clearly indicate that the action is destructive

---

## Dialog Content

The confirmation dialog must include:

- Title: **"Confirmar borrado"**
- Descriptive text explaining the action is irreversible
- A primary action to confirm deletion
- A secondary action to cancel

All user-facing text must be in Spanish.

---

## Interaction Flow

1. User clicks the delete action on a movement
2. Confirmation dialog is displayed
3. User chooses:
   - **Confirm** → deletion is executed
   - **Cancel** → dialog is closed, no action is taken

---

## Behavior on Confirm

- The delete request is sent to the API
- While deleting:
  - The confirm action must be disabled
- On success:
  - The dialog is closed
  - The movements list is reloaded

---

## Error Handling

- If deletion fails:
  - The dialog must remain open
  - A generic error message must be displayed within the dialog

---

## Technical Constraints

- Follow `architecture.md` and `coding-guidelines.md`
- Dialog open/close state handled via local UI state
- Deletion logic must reuse the existing delete hook
- No new dependencies

---

## Out of Scope

- Custom animations
- Undo functionality
- Batch delete
- Different confirmation messages per movement type
