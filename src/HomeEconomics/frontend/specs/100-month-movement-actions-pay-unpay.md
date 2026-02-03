# Spec 100 – Month Movement Actions (Pay / Unpay)

## Goal

Allow the user to mark a month movement as paid or unpaid using explicit actions displayed for each month movement.

---

## Scope

This spec applies only to **MonthMovements** displayed in the current month movements list.

---

## API Contracts

### Pay a Month Movement
POST /api/movement-months/{movementMonthId:int}/month-movements/{monthMovementId:int}/pay

### Unpay a Month Movement
POST /api/movement-months/{movementMonthId:int}/month-movements/{monthMovementId:int}/unpay

See `api-contracts.md` – Movement Months / Pay / Unpay Month Movement

---

## UI Placement

- Each month movement item must expose an **actions area**
- The actions area must be displayed:
  - Immediately **below the amount** of the month movement
  - Visually grouped with the movement item

---

## Actions

### Pending Month Movement

- Must expose a **"Marcar como pagado"** action
- No paid status indicator is required (default state)

### Paid Month Movement

- Must expose a **"Marcar como no pagado"** action
- Must display a visible indicator that the movement is paid

All user-facing text must be in Spanish.

---

## Interaction Flow

### Pay

1. User clicks "Marcar como pagado"
2. Pay API endpoint is called
3. On success:
   - The month movements list is reloaded
   - The movement is treated as paid

### Unpay

1. User clicks "Marcar como no pagado"
2. Unpay API endpoint is called
3. On success:
   - The month movements list is reloaded
   - The movement is treated as pending

---

## States

- While an action request is in progress:
  - The action buttons for that movement must be disabled
- Loading or error state must not affect other movement items

---

## Error Handling

- If the API request fails:
  - A generic error message must be displayed to the user
  - The movement state must remain unchanged

---

## Technical Constraints

- Follow `architecture.md` and `coding-guidelines.md`
- API access via a dedicated service
- Pay/unpay logic must be handled via a hook
- Reload logic must reuse existing data-fetching mechanisms
- No new dependencies

---

## Out of Scope

- Confirmation dialogs for pay/unpay
- Batch actions
- Optimistic updates
- Payment timestamps
- Partial reloads
