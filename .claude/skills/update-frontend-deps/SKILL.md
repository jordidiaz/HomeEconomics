---
name: update-frontend-deps
description: Update, upgrade, or refresh npm/frontend dependencies — analyse outdated packages, classify by semver risk (MAJOR/MINOR/PATCH), auto-apply safe updates, ask before risky ones, and verify the build. Use this skill when the user mentions updating npm packages, upgrading frontend/Node.js/JavaScript/TypeScript dependencies, checking for outdated frontend packages, or keeping JavaScript libraries up to date. Do NOT use for NuGet, .NET, or backend dependency updates — see update-backend-deps for those.
---

## When to use this skill
- You are asked to update, upgrade, or refresh frontend (npm) dependencies.
- You notice outdated packages during another task and the user asks you to handle them.
- You need to check what frontend packages are out of date.
- The user asks about a specific npm package version and whether it can be updated.

## Workflow

### 1) Discover outdated packages

Run from `src/HomeEconomics/frontend/`:

```bash
npm outdated --json
```

This produces JSON with `current`, `wanted`, and `latest` for each outdated package:
- `current` — version currently installed
- `wanted` — highest version satisfying the semver range in `package.json`
- `latest` — newest published stable version

If the output is empty (`{}`), there are no outdated packages — report that and stop.

### 2) Classify each update by semver change

Compare `current` (X.Y.Z) to `latest` (A.B.C):

| Condition | Classification |
|---|---|
| A > X | **MAJOR** |
| A == X and B > Y | **MINOR** |
| A == X and B == Y and C > Z | **PATCH** |

Ignore pre-release suffixes (e.g., `-rc1`, `-beta`) when classifying.

Also note when `wanted != latest`: this means the semver range in `package.json` does not cover the latest version. An explicit `npm install <pkg>@<version>` is needed (not just `npm update`).

### 3) Detect range constraints and coordinated groups

Before updating, check for two patterns:

**Range constraints** — flag any package where `wanted != latest`. The current version range in `package.json` (e.g., `^14.2.4`) doesn't reach `latest`. Flag these in the plan; `npm install <pkg>@<version>` will widen the range automatically.

**Coordinated groups** — some packages must be updated together to a consistent version. Flag and update these as a unit:
- `react` + `react-dom` + `@types/react` + `@types/react-dom`
- `@mui/material` + `@mui/icons-material` + `@mui/lab` (if present)
- `@emotion/react` + `@emotion/styled`
- `@testing-library/react` + `@testing-library/user-event` + `@testing-library/jest-dom` (if present)

If any member of a coordinated group has a MAJOR update, treat the **entire group** as MAJOR.

### 4) Present the update plan

Show a summary table grouped by classification before applying anything:

```
PATCH updates (will be applied automatically):
  Package               Type  Current  → Latest  Range covers?
  @emotion/react        dep   11.11.4  → 11.11.6  ✓

MINOR updates (will be applied automatically):
  Package               Type  Current  → Latest  Range covers?
  @mui/material         dep   5.15.15  → 5.16.2   ✓

MAJOR updates (require your approval):
  Package               Type  Current  → Latest  Range covers?
  next                  dep   14.2.4   → 15.1.0   ✗ (wanted: 14.2.10)

⚠ Coordinated groups — will be updated together:
  react + react-dom + @types/react + @types/react-dom (MINOR)
```

### 5) Apply updates

**Auto-apply MINOR and PATCH updates** without asking. For each package:

```bash
# runtime dependency
npm install <package>@<latest-version>

# devDependency
npm install -D <package>@<latest-version>
```

Update coordinated group members in a single command:

```bash
npm install react@<version> react-dom@<version>
npm install -D @types/react@<version> @types/react-dom@<version>
```

**For MAJOR updates, ask the user before proceeding.** Warn that major version bumps may contain breaking API changes and may require code modifications. Apply only the ones the user explicitly approves.

After all installs, `package-lock.json` is updated automatically by npm.

### 6) Verify the frontend

Run from `src/HomeEconomics/frontend/`, in order:

1. **Build**: `npm run build` — confirms zero TypeScript errors (strict mode is on) and no build-time regressions. TypeScript strict mode plays the same role here as `TreatWarningsAsErrors` does on the backend.
2. **Tests**: `npm run test:ci` — Vitest in single-run mode; confirms all unit and component tests pass. Do NOT use `npm test`, which starts watch mode and will hang.

Do NOT run `npm run e2e` automatically — Playwright E2E requires a running backend instance. Mention in the summary that E2E tests should be run manually.

If the build or tests fail after a specific update, revert that package:

```bash
npm install <package>@<previous-version>
```

Report which package caused the failure and why, then continue with the remaining updates.

### 7) Show final summary

After verification, present a final report:

```
✓ Updated 5 packages (2 minor, 3 patch)
✓ Build succeeded
✓ All unit/component tests passed
ℹ E2E tests not run — requires local backend; run manually with `npm run e2e`

Skipped (user declined):
  - next 14.2.4 → 15.1.0

package-lock.json updated: yes
```

## Decision rules

### When to align coordinated groups
Update all members of a coordinated group to their respective latest versions in a single pass, even if only one member is outdated. Mismatched versions within a group can cause subtle runtime or type errors.

### When to skip a package
- If only pre-release versions are newer (e.g., `-rc1`, `-alpha`), skip unless the user explicitly requests pre-release updates.
- If a package uses an exact version (no `^` or `~` prefix), it may be intentionally pinned — ask the user before updating rather than updating silently.

### devDependencies vs dependencies
Classify and present them separately in the update plan (using the `Type` column). Apply the same MAJOR/MINOR/PATCH rules to both.

### When wanted != latest
If `wanted == latest`, a plain `npm update` would suffice for that package. We still use explicit `npm install <pkg>@<version>` for precision and so `package.json` is updated visibly. If `wanted < latest`, the semver range needs widening — `npm install <pkg>@<version>` handles this automatically.

## Guardrails
- Never apply updates without showing the plan first.
- Never install pre-release versions unless the user explicitly requests it.
- Always use `npm run test:ci`, not `npm test` — the latter starts watch mode and will hang.
- Do not modify `engines` or `volta` fields in `package.json`.
- Do not switch package managers (no yarn, no pnpm).
- Do not run `npm audit fix` as part of this workflow — that is a separate security concern.
- Preserve existing version range prefixes: if `package.json` uses `^`, keep `^`; if it uses `~`, keep `~`. `npm install <pkg>@<version>` respects this automatically.
- Do not add new packages not already in `package.json`.

## Out of scope
- Backend (.NET/NuGet) dependency updates — see `.claude/skills/update-backend-deps/SKILL.md`.
- Node.js version upgrades (changing `.nvmrc`, `engines.node`, or `volta.node`).
- Switching package managers (yarn, pnpm).
- `npm audit` vulnerability remediation (different workflow, different risk profile).
- Adding new packages not currently in `package.json`.
- Updating global npm packages or CLI tools.
