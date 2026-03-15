# Spec 020 – Create Movement

## Goal

Allow the user to create a new movement and immediately see it reflected in the movements list.

---

## API Contract

POST /api/movements

See `api-contracts.md` – Movements / POST /api/movements

---

## UI Placement

- The movement creation form must be displayed:
  - Above the movements list

---

## Form Fields

The form must include the following fields:

### Name
- Text input
- Required

### Amount
- Numeric input
- Required
- Must not be zero

### Type
- Selector
- Required
- Possible values:
  - Income
  - Expense

### Frequency
- Selector
- Required
- Possible values:
  - None
  - Monthly
  - Yearly
  - Custom

---

## Frequency-Specific Behavior

### None
- No additional input is required

### Monthly
- No additional input is required

### Yearly
- A month selector must be displayed
- Only one month can be selected

### Custom
- A month multi-selector must be displayed
- One or more months can be selected

---

## Form Behavior

- Submit action sends the form data to the API
- While submitting:
  - The form must be disabled
- On successful creation:
  - The movements list must be reloaded
  - The form must be reset to its initial state

---

## Validation & Errors

- Required field validation must be handled in the UI
- API errors must be displayed to the user in a generic way
- No inline field-level validation messages are required in this version

---

## States

- Loading state while submitting the form
- Error state if creation fails

---

## Technical Constraints

- Follow `architecture.md` and `coding-guidelines.md`
- API access via a dedicated service
- Form state and submission logic handled via a hook
- No new dependencies

---

## Out of Scope

- Editing existing movements
- Deleting movements
- Advanced validation rules
- Confirmation dialogs
- Optimistic updates