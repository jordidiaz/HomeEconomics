# Spec 090 – Toggle Show Paid Month Movements

## Goal

Allow the user to toggle between viewing only pending month movements (default) and viewing paid month movements.

---

## Scope

This spec applies exclusively to the **current month movements list**.

No backend changes are required.

---

## Default Behavior

- By default, the list must display **only pending month movements**
- Pending movements must NOT display any explicit status label

---

## UI Requirements

- A toggle control must be provided to switch the list mode
- Toggle label must be: **"Mostrar pagados"**
- The toggle must be displayed near the current month movements list

All user-facing text must be in Spanish.

---

## Toggle Behavior

### Toggle OFF (Default)

- Only pending month movements are displayed
- No payment status indicator is shown

### Toggle ON ("Mostrar pagados")

- Only paid month movements are displayed
- Paid movements must display a visible indicator that they are paid

---

## Filtering Rules

- Filtering must be performed entirely in the client application
- No additional API calls must be triggered when toggling
- The full dataset must be fetched once and filtered locally

---

## States

- Toggle must be available after data is loaded
- Toggle must not be displayed if the list is empty

---

## Technical Constraints

- Follow `architecture.md` and `coding-guidelines.md`
- Filtering logic must be handled in the hook layer
- No changes to API services
- No new dependencies

---

## Out of Scope

- Showing both paid and pending movements at the same time
- Persisting toggle state across sessions
- Server-side filtering
- Animations or transitions
