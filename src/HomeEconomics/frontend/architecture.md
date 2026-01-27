# Frontend Architecture

This document defines the non-negotiable architectural rules for this SPA.
All code must conform to this structure and principles.

---

## 1. Tech Stack

- Framework: Next.js (App Router)
- Language: TypeScript (strict mode)
- UI Library: MUI (Material UI)
- Styling: MUI system only (sx, theme)
- Data fetching: fetch (no external query libraries unless explicitly approved)

---

## 2. High-Level Structure

- /app → Routing, layouts, and page composition
- /components → Reusable presentational components
- /hooks → Reusable logic and side effects
- /services → API access layer (backend communication)
- /types → Shared TypeScript domain models
- /styles → Theme and global styling
- /specs → Feature specifications (input for Codex)


---

## 3. Layer Responsibilities

### 3.1 app/
- Defines routes and layouts only
- No business logic
- No direct API calls
- Pages orchestrate hooks and components

**Allowed**
- Layout composition
- Wiring hooks to UI
- Navigation

**Forbidden**
- fetch calls
- Data transformation logic

---

### 3.2 services/
- Single source of truth for backend communication
- One service per backend resource
- Uses `fetch` and handles HTTP concerns

**Rules**
- No UI imports
- No React imports
- No state management
- Returns typed DTOs

Example:
- `UsersService`
- `AuthService`

---

### 3.3 hooks/
- Encapsulate side effects and state
- Consume services
- Expose UI-ready data

**Rules**
- One hook per use case
- Handles loading and error states
- No JSX
- No direct fetch (services only)

Example:
- `useUsers`
- `useUserDetails`

---

### 3.4 components/
- Presentational and reusable UI pieces
- Stateless when possible

**Rules**
- No direct API access
- Minimal logic
- Receives data via props

---

### 3.5 types/
- Shared domain models
- Aligned with backend DTOs
- No UI-specific types

---

### 3.6 styles/
- MUI theme configuration
- Global styles only
- No component-level CSS files

---

## 4. Data Flow

Page (app/) -> Hook (hooks/) -> Service (services/) -> Backend API

Reverse flow is forbidden.

---

## 5. Error Handling

- Services return data or throw errors
- Hooks map errors to UI-friendly states
- Pages/components render error UI

---

## 6. Naming Conventions

- Services: `XService`
- Hooks: `useX`
- Types: `X`, `XDto` if needed
- Files use kebab-case
- Components use PascalCase

---

## 7. Dependency Rules

- app -> hooks -> services -> backend
- components can be used by app or other components
- services MUST NOT import from:
  - app
  - components
  - hooks

---

## 8. Testing Strategy (Optional but Recommended)

- Unit test hooks and services
- No snapshot tests
- Pages tested only for wiring, not logic

---

## 9. Allowed Libraries

Only the following are allowed by default:
- next
- react
- @mui/material
- @mui/icons-material

Any new dependency must be explicitly approved and documented.

---

## 10. Change Policy

If a feature requires breaking these rules:
1. Update this document
2. Commit the change
3. Implement the feature

Specs must never override this architecture.
