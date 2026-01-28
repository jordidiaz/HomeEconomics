# Codex Instructions

You are an AI coding agent working inside this repository.
Your role is to implement features deterministically, not to explore alternatives.

This repository follows a strict spec-driven development approach.

---

## 1. Source of Truth

You MUST treat the following files as authoritative:

1. architecture.md
2. coding-guidelines.md
3. specs/*.md
4. api-contracts.md (if present)

If there is a conflict:
- architecture.md overrides everything
- coding-guidelines.md overrides specs
- specs define WHAT to build, never HOW

---

## 2. How to Work

Before writing any code, you MUST:
1. Read the relevant spec
2. Read architecture.md
3. Read coding-guidelines.md
4. Inspect existing patterns in the codebase

You must follow existing conventions even if alternatives exist.

---

## 3. Scope Discipline

You MUST:
- Implement only what is explicitly described in the spec
- Respect the "Out of Scope" section
- Avoid anticipatory abstractions
- Avoid speculative features

You MUST NOT:
- Add pagination, caching, retries, or optimizations unless specified
- Introduce new libraries unless explicitly requested
- Refactor unrelated code

---

## 4. Architecture Rules (Non-Negotiable)

- No direct API calls outside /services
- No business logic in components
- Hooks orchestrate data fetching and state
- Services only handle HTTP concerns
- Pages compose hooks and components

If a spec appears to violate these rules, STOP and report the conflict.

---

## 5. Coding Rules (Strict)

- TypeScript only
- No `any`
- Explicit types in public APIs
- One primary export per file
- Files must follow naming conventions

Generated code must be clean, minimal, and readable.

---

## 6. Consistency Over Novelty

You MUST prefer:
- Existing services over new ones
- Existing hooks over new abstractions
- Existing patterns over new patterns

If a pattern already exists, reuse it verbatim.

---

## 7. Error Handling

- Services throw errors on non-2xx responses
- Hooks map errors to UI states
- UI must expose loading and error states

Silent failures are forbidden.

---

## 8. Language & Internationalization

### Source Code Language
- All source code MUST be written in English:
  - File names
  - Variables
  - Functions
  - Types
  - Comments

### User-Facing Language
- All user-facing text MUST be written in Spanish.

User-facing text includes (non-exhaustive):
- Visible HTML text content
- Button labels
- Form labels
- Placeholders
- Helper texts
- Error messages
- Empty and loading state messages
- `aria-label`, `title`, and `alt` attributes when exposed to the user

### Explicit Exclusions
- Internal log messages
- Exception messages not rendered in the UI
- API error payloads (unless explicitly mapped to UI)

Mixing languages in user-facing text is forbidden.

## 9. Documentation

When introducing a new feature:
- Update README.md if setup or usage changes
- Do NOT add inline comments unless intent is non-obvious

---

## 10. When in Doubt

If any requirement is ambiguous:
- Do NOT guess
- Do NOT invent
- Ask for clarification or point out the ambiguity

---

## 11. Definition of Done

A task is complete only if:
- The spec is fully implemented
- All architectural rules are respected
- No unrelated code was modified
- No new dependencies were added
