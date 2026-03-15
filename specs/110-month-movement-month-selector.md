# Spec 110 – Month Movement Selector (Current / Next)

## Goal

Allow the user to switch between the current month and the next movement month, creating the next month if it does not exist, and always displaying the movements for the selected month.

---

## Scope

This spec applies to:
- The month movements list
- The month resolution logic used to fetch MonthMovements

---

## API Contracts

### Get Movement Month
GET /api/movement-months/{year:int}/{month:int}

### Create Movement Month
POST /api/movement-months

See `api-contracts.md` – Movement Months

---

## UI Placement

- A **month selector component** must be displayed:
  - Above the month movements list
  - Below any global page or section title

---

## Component Behavior

The component must allow interaction with **two possible months**:
- The current month
- The next month

---

## Month Resolution

- The current month is determined at runtime using the system date
- The next month is computed relative to the current month
  - Year rollover must be handled correctly (December → January)

---

## Interaction Rules

### When `nextMovementMonthExists` is TRUE

- The user must be able to:
  - Select the current month
  - Select the next month
- Switching selection must:
  - Update the selected month
  - Reload the month movements for the selected month

---

### When `nextMovementMonthExists` is FALSE

- The next month option must be displayed as unavailable
- The user must be able to trigger creation of the next month
- On creation:
  - The next month must be created using the POST endpoint
  - The newly created month becomes selectable
  - The newly created month is automatically selected
  - The month movements list is loaded for the new month

---

## Data Loading Rules

- Month movements must always be loaded for the **currently selected month**
- Changing the selected month must trigger a reload
- No stale data from a previously selected month must be displayed

---

## States

- Loading state while switching months
- Loading state while creating the next month
- Error state if month creation fails
- The selector must be disabled while an action is in progress

---

## Error Handling

- If loading a movement month fails:
  - A generic error message must be displayed
- If creating the next month fails:
  - The user must remain on the current month
  - A generic error message must be displayed

---

## Technical Constraints

- Follow `architecture.md` and `coding-guidelines.md`
- Month selection state must be handled via a dedicated hook
- API access via a dedicated service
- Date calculation logic must not be duplicated across components
- No new dependencies

---

## Out of Scope

- Navigation to previous months
- Displaying more than two months
- Persisting selected month across sessions
- Prefetching month data
- Animations or transitions
