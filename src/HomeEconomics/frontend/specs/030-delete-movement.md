# Spec 030 – Delete Movement

## Goal

Allow the user to delete an existing movement from the movements list.

---

## API Contract

DELETE /api/movements/{id:int}

See `api-contracts.md` – Movements / DELETE /api/movements/{id}

---

## UI Requirements

- Each movement item in the movements list must expose a delete action
- The delete action must be clearly identifiable as destructive

---

## Interaction

- When the user clicks the delete action:
  - The movement is deleted via the API
- No confirmation dialog is required in this version

---

## Behavior on Success

- After successful deletion:
  - The movements list must be reloaded
  - The deleted movement must no longer appear in the list

---

## Error Handling

- If deletion fails:
  - A generic error message must be displayed to the user
- If the API returns a domain conflict (409):
  - The error must be shown to the user in a generic way
  - No special-case handling is required

---

## States

- Loading state while the delete request is in progress
- The delete action must be disabled while the request is pending

---

## Technical Constraints

- Follow `architecture.md` and `coding-guidelines.md`
- API access via a dedicated service
- Deletion logic handled via a hook
- No new dependencies

---

## Out of Scope

- Confirmation dialogs
- Optimistic updates
- Bulk delete
- Undo functionality
- Soft delete or visual transitions
