# Spec 130 – Edit Month Movement Amount

## Goal

Allow the user to edit the amount of a month movement from the current month movements list.

---

## Scope

This spec applies only to **MonthMovements** displayed in the current month movements list.
The edit action must be available for both paid and pending movements.

---

## API Contract

POST /api/movement-months/{movementMonthId:int}/month-movements/{monthMovementId:int}/update-amount

See `api-contracts.md` – Movement Months / Update Movement Amount

---

## UI Placement

- Each month movement item must expose an **actions area**
- The edit action must be displayed:
  - Immediately **below the amount** of the month movement
  - Alongside the existing pay/unpay actions

---

## Dialog Content

The edit dialog must include:

- Title: **"Editar importe"**
- A single **amount** input field
- Primary action: **"Aceptar"**
- Secondary action: **"Cancelar"**

The amount field must be prefilled with the current amount of the month movement.
All user-facing text must be in Spanish.

---

## Interaction Flow

1. User clicks the edit action
2. The edit dialog opens with the current amount prefilled
3. The user modifies the amount
4. The user clicks **"Aceptar"**
5. The update amount API endpoint is called
6. On success:
   - The dialog is closed
   - The month movements list is reloaded

If the user clicks **"Cancelar"**:
- The dialog is closed
- No API call is made

---

## States

- While the update request is in progress:
  - The dialog actions must be disabled
  - The edit action for that movement must be disabled
- Loading or error state must not affect other movement items

---

## Error Handling

- If the API request fails:
  - The dialog must remain open
  - A generic error message must be displayed within the dialog
  - The movement amount must remain unchanged

---

## Technical Constraints

- Follow `architecture.md` and `coding-guidelines.md`
- API access via a dedicated service
- Update logic must be handled via a hook
- Reload logic must reuse existing data-fetching mechanisms
- No new dependencies

---

## Out of Scope

- Inline editing without a dialog
- Editing other fields
- Optimistic updates
- Batch edit
- Custom validation rules beyond existing amount validation
