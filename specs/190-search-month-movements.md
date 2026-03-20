# Spec 190 – Search Month Movements

## Goal

Allow the user to filter the current month movements list by typing a search term that matches against the movement name or amount.

---

## Scope

This spec applies exclusively to the **current month movements list**.

No backend changes are required.

---

## UI Requirements

- A text input field must be displayed between the **"Mostrar pagados"** toggle and the month movements list
- Placeholder text must be: **"Buscar por nombre o importe"**
- The input must span the full available width
- A clear button (✕) must appear inside the input when it contains text, allowing the user to reset the search with one click

All user-facing text must be in Spanish.

---

## Search Behavior

- Filtering must begin immediately as the user types (no submit button)
- The search term must match against:
  - **Name**: partial, case-insensitive match (e.g., "aho" matches "Ahorro")
  - **Amount**: partial match against the formatted amount string (e.g., "1.0" matches "1.000,00")
- A movement is shown if **either** the name or the amount matches the search term
- An empty search term must display all movements (no filter applied)

---

## Interaction with Paid Toggle

- The search filter must compose with the existing paid/pending toggle
- Both filters are applied together: first the paid/pending filter, then the search filter (or vice versa — the result must be the same)
- Changing the toggle must not clear the search term
- The search input must remain visible regardless of toggle state

---

## States

- The search input must be visible whenever the movement list is loaded and non-empty (same visibility conditions as the "Mostrar pagados" toggle)
- If the search yields no results, display an informational message: **"No se encontraron movimientos"**
- The search input must not be displayed while data is loading or when an error occurred

---

## Technical Constraints

- Follow `architecture.md` and `coding-guidelines.md`
- Filtering logic must be handled in the hook layer
- No changes to API services
- No new dependencies
- No debounce required (dataset is small enough for instant filtering)

---

## Out of Scope

- Server-side search or filtering
- Search across multiple months
- Search by movement type (income/expense)
- Persisting search term across page navigation or sessions
- Advanced search syntax (regex, operators)
