---
name: update-deps
description: Update, upgrade, or refresh .NET/NuGet backend dependencies — analyse outdated packages, classify by semver risk (MAJOR/MINOR/PATCH), auto-apply safe updates, ask before risky ones, and verify the build. Use this skill whenever the user mentions updating packages, upgrading NuGet dependencies, checking for outdated packages, or keeping backend libraries up to date, even if they don't use the word "dependency".
---

## When to use this skill
- You are asked to update, upgrade, or refresh backend (NuGet) dependencies.
- You notice outdated packages during another task and the user asks you to handle them.
- You need to check what backend packages are out of date.
- The user asks about a specific package version and whether it can be updated.

## Workflow

### 1) Discover outdated packages

Run against the solution root so every project is covered in one pass:

```bash
dotnet list package --outdated --format json
```

The `--format json` flag (available since .NET 8) produces machine-parseable output. Parse the JSON to extract, per project, each outdated package's **current version** and **latest version**.

If `--format json` is unavailable or fails, fall back to:

```bash
dotnet list package --outdated
```

and parse the tabular output instead. If there are no outdated packages, report that to the user and stop.

### 2) Classify each update by semver change

Compare current and latest versions for every outdated package. Given `current = X.Y.Z` and `latest = A.B.C`:

| Condition | Classification |
|---|---|
| A > X | **MAJOR** |
| A == X and B > Y | **MINOR** |
| A == X and B == Y and C > Z | **PATCH** |

Ignore pre-release suffixes (e.g., `-preview`, `-rc1`) when classifying — treat them as equivalent to their base version numbers.

### 3) Detect version drift

Before updating, check whether the same package appears in multiple projects at different versions (e.g., `Microsoft.AspNetCore.Mvc.Testing` at 9.0.11 in one project and 10.0.1 in another). Flag these to the user — a drift within the same major is safe to align, a drift across majors requires approval.

### 4) Present the update plan

Show the user a summary table grouped by classification before applying anything:

```
PATCH updates (will be applied automatically):
  Package                          Project                        Current  → Latest
  Serilog.Sinks.Console            HomeEconomics.csproj           6.1.0    → 6.1.1

MINOR updates (will be applied automatically):
  Package                          Project                        Current  → Latest
  FluentValidation                 HomeEconomics.csproj           12.0.0   → 12.1.1

MAJOR updates (require your approval):
  Package                          Project                        Current  → Latest
  xunit                            Domain.UnitTests.csproj        2.9.3    → 3.0.0

⚠ Version drift detected:
  Microsoft.AspNetCore.Mvc.Testing: 9.0.11 (Domain.UnitTests) vs 10.0.1 (HomeEconomics.UnitTests, HomeEconomics.IntegrationTests)
```

### 5) Apply updates

**Auto-apply MINOR and PATCH updates** without asking. For each package, run:

```bash
dotnet add <project-path> package <PackageName> --version <LatestVersion>
```

**For MAJOR updates, ask the user before proceeding.** Warn that major version bumps may contain breaking API changes and may require code modifications. Apply only the ones the user explicitly approves.

When the same package appears in multiple projects, update all occurrences to the same latest version to eliminate drift.

### 6) Verify the solution

After all updates are applied, run these checks in order:

1. **Restore**: `dotnet restore`
2. **Build**: `dotnet build --no-restore` — confirm zero errors and zero warnings. This solution has `TreatWarningsAsErrors` enabled globally in `Directory.Build.props`, so new package versions introducing deprecation warnings will cause a build failure.
3. **Test**: `dotnet test --no-build` — confirm all tests pass.

If the build or tests fail after a specific update, revert that package to its previous version, report which package caused the failure and why, then continue with remaining updates.

### 7) Show final summary

After verification, present a final report:

```
✓ Updated 3 packages (1 minor, 2 patch)
✓ Build succeeded
✓ All tests passed

Skipped (user declined):
  - xunit 2.9.3 → 3.0.0

Remaining drift:
  - (none, or list any that remain)
```

## Decision rules

### When to align drifted versions
- If the drift is within the same major version, align to the latest automatically during this update run.
- If the drift spans major versions (e.g., 9.x vs 10.x), treat the lower version as a MAJOR update candidate and ask the user before aligning.

### When to skip a package
- If a package has no newer **stable** version (only pre-release), skip it unless the user explicitly asks for pre-release updates.
- If you have reason to believe a package is intentionally pinned (e.g., a comment in the .csproj or a known compatibility constraint), ask the user before updating it rather than updating silently.

### When to run integration tests
- Always run the full test suite (`dotnet test`) after updates, not just unit tests. Package updates can surface issues at any layer, including persistence and HTTP integration behaviour.

## Guardrails
- Never apply updates without showing the plan first.
- Never install pre-release versions unless the user explicitly requests it.
- Always verify the build after updates — `TreatWarningsAsErrors` is on, so new package versions can introduce new warnings that become blocking errors.
- If `dotnet restore` fails due to a version conflict, diagnose and report to the user before attempting further updates.
- Preserve existing `PrivateAssets` and `IncludeAssets` metadata on PackageReference entries when updating versions.
- Do not introduce Central Package Management (`Directory.Packages.props`) as part of this workflow unless the user specifically requests it.

## Out of scope
- Frontend (npm) dependency updates.
- .NET SDK or runtime version upgrades (e.g., changing `NetTargetVersion` in `Directory.Build.props`).
- Introducing Central Package Management (`Directory.Packages.props`).
- NuGet source or feed configuration changes.
- Updating `global.json` or dotnet tooling manifests.
