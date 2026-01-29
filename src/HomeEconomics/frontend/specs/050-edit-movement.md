# Spec 050 – Edit Movement

## Goal

Allow the user to edit an existing movement by loading its data into the movement form and updating it on save.

---

## API Contract

PUT /api/movements/{id:int}

See `api-contracts.md` – Movements / PUT /api/movements/{id}

---

## UI Requirements

- Each movement item in the movements list must expose an edit action
- The edit action must be clearly distinguishable from the delete action

---

## Interaction Flow

1. User clicks the edit action on a movement
2. The movement creation form is populated with the selected movement data
3. The user modifies the fields
4. The user saves the form
5. The movement is updated via the API

---

## Form Behavior (Edit Mode)

- The same form used for creation must be reused for editing
- The form must enter an **edit mode** when populated with an existing movement
- While in edit mode:
  - The save action updates the existing movement
  - The movement identifier must be preserved
- No separate edit page or dialog is required

---

## Behavior on Save

- On successful update:
  - The form exits edit mode
  - The form is reset to its initial (create) state
  - The movements list is reloaded

---

## Validation & Errors

- UI-level required field validation remains unchanged
- API errors must be displayed to the user in a generic way
- No additional validation rules are required for editing

---

## States

- Loading state while the update request is in progress
- Save action must be disabled while submitting

---

## Technical Constraints

- Follow `architecture.md` and `coding-guidelines.md`
- API access via a dedicated service
- Creation and update logic must be handled via a shared hook or coordinated hooks
- No new dependencies

---

## Out of Scope

- Cancel edit action
- Confirmation dialog before saving
- Partial updates
- Optimistic updates
- Version conflict handling
