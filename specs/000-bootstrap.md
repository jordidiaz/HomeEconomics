# Spec 000 – Frontend Bootstrap

## Goal

Bootstrap the frontend SPA with a clean, documented, and production-ready foundation.
This spec establishes the baseline architecture, tooling, and patterns that all future features will build upon.

No business features are implemented in this spec.

---

## Scope

This spec includes:
- Project creation
- Folder structure
- Base configuration
- Documentation
- One minimal example to validate the setup

This spec explicitly excludes any real product functionality.

---

## Tech Stack

- Framework: Next.js (App Router)
- Language: TypeScript (strict)
- UI Library: MUI (Material UI)
- Styling: MUI theme and `sx`
- Data fetching: native `fetch`

---

## Folder Structure

The following structure MUST be created:

```
/app
  /layout.tsx
  /page.tsx

/components
/hooks
/services
/types
/styles
/specs (already exists)

architecture.md (already exists)
coding-guidelines.md (already exists)
codex.md (already exists)
README.md
```

Empty folders must be committed.

---

## Configuration Requirements

### TypeScript
- `strict: true` enabled
- No `any` usage

### Next.js
- App Router enabled
- Default ESLint configuration is acceptable

---

## Styling & Theme

- Configure a base MUI theme in `/styles/theme.ts`
- Wrap the application with `ThemeProvider`
- No custom CSS files

Theme should include:
- Primary color
- Secondary color
- Base typography

---

## Example Feature (Minimal)

A minimal example must be implemented to validate the architecture.

### Example Page
- Route: `/`
- Displays a simple title using MUI components
- No API calls
- No business logic

Purpose: verify routing, MUI setup, and layout composition.

---

## Documentation

### README.md must include:

- Project purpose
- Tech stack
- Requirements (Node version)
- Install instructions
- Dev command
- Build command

Example sections:
- `npm install`
- `npm run dev`
- `npm run build`

---

## Architectural Constraints

All code MUST comply with:
- architecture.md
- coding-guidelines.md
- codex.md

This spec must not introduce exceptions.

---

## Out of Scope

- API integration
- Authentication
- State management libraries
- Testing setup
- CI/CD
- Deployment configuration

These will be handled in later specs.

---

## Definition of Done

This spec is complete when:
- The project runs locally
- The folder structure is present and committed
- MUI is correctly configured
- Documentation is complete
- No business logic exists in the codebase
- No unused dependencies are added
