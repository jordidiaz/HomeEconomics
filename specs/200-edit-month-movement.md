# Spec 200 – Edit Month Movement

## Goal

Allow the user to edit the name, amount, and type of a month movement from the current month movements list. This extends the current edit behavior (spec 130), which only supports editing the amount.

---

## Scope

This spec applies only to **MonthMovements** displayed in the current month movements list.
The edit action must be available for both paid and pending movements.

Replaces the edit behavior from spec 130. The previous endpoint (`update-amount`) is marked obsolete but not removed.

---

## API Contract

POST /api/movement-months/{movementMonthId:int}/month-movements/{monthMovementId:int}/update

See `api-contracts.md` – Movement Months / Update Month Movement (section to be added).

**Contract rules:**
- `name` is required, must not be empty or whitespace. Max length: 30 characters (aligned with `Movement.MovementNameMaxLength`).
- `amount` must be ≥ `Movement.MinAmount` (0).
- `type` must be a valid `MovementType` value (0 = Income, 1 = Expense).
- Returns **404** if the month movement or movement month does not exist.
- Validation errors return **400** in `ProblemDetails` format.
- On success, returns the full updated `MovementMonth` response.

**Obsolete endpoint:**
- POST `.../update-amount` remains but is considered obsolete. Removal is out of scope.

---

## UI Placement

- The edit action replaces the current "editar importe" action.
- Displayed in the same position: immediately **below the amount**, alongside existing pay/unpay actions.

---

## Dialog Content

The edit dialog must include:

- Title: **"Editar movimiento"**
- Text field **"Nombre"** — required, max 30 characters
- Numeric field **"Cantidad"** — required
- Select field **"Tipo"** — required, options: "Ingreso" / "Gasto"
- Primary action: **"Aceptar"**
- Secondary action: **"Cancelar"**

All three fields must be prefilled with the current values of the month movement.
All user-facing text must be in Spanish.

---

## Interaction Flow

1. User clicks the edit action
2. The dialog opens with name, amount, and type prefilled with current values
3. The user modifies one or more fields
4. The user clicks **"Aceptar"**
5. The update endpoint is called
6. On success:
   - The dialog is closed
   - The month movements list is reloaded

If the user clicks **"Cancelar"**:
- The dialog is closed
- No API call is made

---

## States

- While the update request is in progress:
  - All dialog fields must be disabled
  - The dialog actions must be disabled
  - The edit action for that movement must be disabled
- Loading or error state must not affect other movement items

---

## Error Handling

- If the API request fails:
  - The dialog remains open
  - A generic error message is displayed within the dialog
  - The movement values remain unchanged

---

## Technical Constraints

- Follow `architecture.md` and `coding-guidelines.md`
- API access via the dedicated service (`MovementMonthsService`)
- Update logic must be handled via a hook
- Reload logic must reuse existing data-fetching mechanisms
- Replace `edit-month-movement-amount-dialog.tsx` with a new `edit-month-movement-dialog.tsx` component
- Name and Type fields must follow the same UI patterns as `add-month-movement-form.tsx`
- No new dependencies

---

## Out of Scope

- Inline editing without a dialog
- Editing paid status from this dialog
- Optimistic updates
- Batch editing
- Removal of the old `update-amount` endpoint
- Frontend validation beyond required fields, name max length, and valid type
