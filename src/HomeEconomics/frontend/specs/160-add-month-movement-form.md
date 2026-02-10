# Spec 160 – Add Month Movement Form

## Goal

Allow the user to create a new month movement directly from the current month movements view.

---

## Scope

This spec applies only to the current month movements list.

---

## API Contract

POST /api/movement-months/{movementMonthId:int}/month-movements

See `api-contracts.md` – Movement Months / Add a movement to a month

---

## UI Placement

- A new form must be displayed **just above** the current month movements list
- The form must be visually grouped with the list

---

## Form Fields

The form must include the following fields:

- **Name**
- **Amount**
- **Type**

All user-facing text must be in Spanish.

---

## Actions

- **"Cancelar"**
  - Resets/clears the form fields
- **"Aceptar"**
  - Submits the form and creates the month movement

---

## Interaction Flow

1. User fills the form
2. User clicks **"Aceptar"**
3. The create API endpoint is called
4. On success:
   - The form is cleared
   - The month movements list is reloaded

If the user clicks **"Cancelar"**:
- The form is cleared
- No API call is made

---

## Validation & Errors

- The form must keep existing validation rules for required fields
- If the API request fails:
  - A generic error message must be displayed
  - The form values must remain unchanged

---

## States

- While the create request is in progress:
  - The form actions must be disabled

---

## Technical Constraints

- Follow `architecture.md` and `coding-guidelines.md`
- API access via a dedicated service
- Create logic must be handled via a hook
- Reload logic must reuse existing data-fetching mechanisms
- No new dependencies

---

## Out of Scope

- Additional fields beyond Name, Amount, and Type
- Optimistic updates
- Batch creation
- Inline validation beyond required fields
