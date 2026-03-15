# Spec 060 – Cancel Movement Creation or Edit

## Goal

Allow the user to cancel the creation or editing of a movement and return the form to its initial state without applying any changes.

---

## Scope

This spec applies to:
- Creating a new movement
- Editing an existing movement

The same cancel behavior must be used in both cases.

---

## UI Requirements

- The movement form must expose a cancel action
- The cancel action must be clearly distinguishable from the save action

---

## Interaction Flow

### While Creating a Movement

1. User fills in the movement form
2. User clicks the cancel action
3. The form is reset to its initial empty state

### While Editing a Movement

1. User enters edit mode for an existing movement
2. User modifies one or more fields
3. User clicks the cancel action
4. The form exits edit mode
5. The form is reset to its initial (create) state

---

## Behavior

- Canceling must:
  - Discard all unsaved changes
  - Reset all form fields
  - Exit edit mode if active
- No API calls must be triggered when canceling

---

## States

- The cancel action must be available at all times
- Canceling must be possible even if the form is partially filled

---

## Technical Constraints

- Follow `architecture.md` and `coding-guidelines.md`
- Cancel logic must be handled locally in the form or form-related hook
- No changes to API services are required
- No new dependencies

---

## Out of Scope

- Confirmation dialogs for canceling
- Dirty form warnings
- Auto-save
- Undo functionality
