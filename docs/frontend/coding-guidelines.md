# Coding Guidelines

This document defines code-level conventions and rules.
All code must follow these guidelines unless explicitly stated otherwise.

---

## 1. Language & Compiler Settings

- TypeScript is mandatory
- `strict: true` must be enabled
- No `any`
- Prefer explicit types over inference in public APIs

---

## 2. File & Folder Conventions

- File names use `kebab-case`
- React components use `PascalCase`
- Hooks start with `use`
- One primary export per file

---

## 3. Components

### Rules
- Components are presentational by default
- No direct API calls
- No side effects (`useEffect`) unless explicitly justified
- Logic should be extracted to hooks

### Preferred Pattern
```tsx
type Props = {
  users: User[];
};

export function UsersTable({ users }: Props) {
  return (...)
}
```

---

## 4. Hooks

### Rules
- One hook per use case
- Hooks manage:
  - data fetching
  - loading state
  - error state
- Hooks must not return JSX

### Preferred Pattern
```ts
export function useUsers() {
  const [data, setData] = useState<User[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<Error | null>(null);

  ...
  return { data, loading, error };
}
```

---

## 5. Services (API Layer)

### Rules
- All backend access goes through services
- Services use `fetch`
- No React imports
- No state
- No retries unless specified in a spec

### Error Handling
- Non-2xx responses throw errors
- Errors must include HTTP status and message

### Preferred Pattern
```ts
export class UsersService {
  static async getAll(): Promise<User[]> {
    const response = await fetch("/api/users");

    if (!response.ok) {
      throw new Error(`Failed to fetch users (${response.status})`);
    }

    return response.json();
  }
}
```

---

## 6. Types

### Rules
- Types live in `/types`
- Align with backend DTOs
- No UI-specific fields

### Naming
- Domain models: `User`
- Backend DTOs (if different): `UserDto`

---

## 7. Data Transformation

- Data transformation happens in hooks
- Services return raw backend data
- Components receive UI-ready data

---

## 8. State Management

- Prefer local state
- No global state library unless explicitly approved
- Lift state only when required by multiple components

---

## 9. Styling

- Use MUI components and `sx` prop
- No inline styles
- No CSS files per component
- Theme values must be used for spacing and colors

---

## 10. Error & Loading UI

- Loading and error states are mandatory for async flows
- No silent failures
- Errors must be user-visible (basic message is enough)

---

## 11. Logging & Debugging

- No `console.log` in committed code
- Temporary logs must be removed
- Use descriptive error messages

---

## 12. Comments & Documentation

- Prefer readable code over comments
- Comment only when intent is non-obvious
- Public functions must have clear naming

---

## 13. Testing (If Present)

- Test behavior, not implementation
- Avoid snapshot tests
- Mock services, not hooks

---

## 14. AI Usage Rules (Important)

- Codex must follow this document strictly
- Do not introduce new patterns
- Do not add libraries without approval
- Reuse existing abstractions

If a change is needed:
1. Update this file
2. Commit
3. Implement the change
