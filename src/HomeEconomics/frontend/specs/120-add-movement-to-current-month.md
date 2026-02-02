# Spec 120 – Add Movement to Current Month

## Goal

Allow the user to add an existing movement to the current month directly from the movements list.

---

## Scope

This spec applies to:
- The movements list actions area
- The current month movements list reload flow

---

## API Contract

POST /api/movement-months/{movementMonthId:int}/month-movements

See `api-contracts.md` – Movement Months / POST /api/movement-months/{movementMonthId}/month-movements

### Request Body

The request body must be built from the selected movement:
- `name` (string)
- `amount` (decimal)
- `type` (MovementType)

---

## UI Placement

- Each movement item in the movements list must expose an **add-to-current-month action**.
- The action icon must be placed **adjacent to the Edit and Delete actions**.

---

## Interaction Flow

1. User clicks the add-to-current-month action.
2. The API endpoint is called with the current `movementMonthId`.
3. On success:
   - The current month movements list is reloaded.

All user-facing text must be in Spanish.

---

## States

- While the request is in progress:
  - The action icon for that movement is disabled.
- Other movement items must remain interactive.

---

## Error Handling

- If the API request fails:
  - A generic error message must be displayed to the user.
  - The current month movements list must remain unchanged.

---

## Technical Constraints

- Follow `architecture.md` and `coding-guidelines.md`
- API access via a dedicated service
- Action state and request handling via a hook
- Reload current month movements via existing data-fetching mechanisms
- No new dependencies

---

## Out of Scope

- Adding movements to months other than the current one
- Batch add actions
- Confirmation dialogs
- Optimistic updates
- Editing the movement payload before submission
