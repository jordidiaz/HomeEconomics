# Spec 070 – Split Layout for Movements Page

## Goal

Reorganize the movements page layout so that the movement form and movements list are displayed on the right half of the screen, leaving the left half available for future components.

---

## Scope

This spec affects only:
- The layout and positioning of existing components
- The movements page composition

No functional or behavioral changes are introduced.

---

## Layout Requirements

- The page must be visually divided into two vertical sections:
  - Left section: empty placeholder
  - Right section: movements content

- The movements content includes:
  - Page title ("Movimientos")
  - Movement creation/edit form
  - Movements list

---

## Positioning Rules

- The right section must occupy approximately 50% of the horizontal space
- The left section must occupy the remaining space
- The movements content order within the right section must remain unchanged:
  1. Page title
  2. Movement form
  3. Movements list

---

## Behavior

- All existing behavior must remain unchanged
- Create, edit, delete, cancel, and confirmation flows must continue to work as before
- No data fetching logic must be modified

---

## Technical Constraints

- Follow `architecture.md` and `coding-guidelines.md`
- Layout changes must be implemented at the page or layout level
- No changes to services, hooks, or business logic
- No new dependencies

---

## Out of Scope

- Implementing content in the left section
- Responsive behavior adjustments
- Animations or transitions
- Collapsible or resizable panels
