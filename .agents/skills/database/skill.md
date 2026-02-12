---
name: database
description: Guide database and migration decisions for the HomeEconomics backend, focusing on *when* and *why* to change schema, mappings, or persistence behavior
---

## When to use this skill
- You need to change database schema or migrations.
- You are deciding whether a change belongs in the domain model, persistence mapping, or query layer.
- You are adjusting how data is queried or stored for performance or correctness.

## Core decision rules

### 1) Decide if the change is a schema change
- **Schema change** when data shape or constraints must evolve (new columns, relationships, indexes, or required fields).
- **Not a schema change** when only behavior changes in the domain or feature layer without altering stored data.

### 2) Decide where mapping rules belong
- **EF configuration** is for persistence concerns (table/column names, relationships, indexes, conversions).
- **Domain model** holds business meaning; avoid persistence-driven properties if they do not represent domain concepts.
- EF configuration reflects the database schema, but does not define domain meaning.

### 3) Decide when to add or update a migration
- Add a migration **only** when the schema must change in production or shared environments.
- Avoid migrations for purely local/test data adjustments; prefer seeded data or fixtures where appropriate.

### 4) Decide how to handle breaking changes
- If a migration would break existing data, introduce a **transition strategy** (nullable columns, backfill, or multi-step migration) rather than a single destructive change.
- Prefer backward-compatible schema updates when deployments may be staggered.

### 5) Decide on query shape vs. domain change
- If a query is slow due to shape, optimize in persistence (indexes, projections) without changing domain semantics.
- If slow because the domain model is missing a concept, update the domain model first and then adjust persistence.

### 6) Decide on ownership of data integrity
- **Database constraints** protect data at rest (unique constraints, foreign keys).
- **Domain invariants** protect data in motion (business rules before persistence).
- Use both where the rule is critical; do not rely on only one layer for critical integrity.

### 7) Avoid opportunistic schema changes
- Do not modify schema “while you’re there” unless it directly supports the current feature.
- Unrelated schema cleanups should be isolated, reviewed, and deployed independently.

## Guardrails
- Keep migrations minimal and reversible when possible.
- Avoid introducing persistence-specific details into domain entities.
- Prefer explicit, intentional schema evolution over ad-hoc changes.
- Avoid column or table deletion/renames without a deprecation period and data migration plan.

## Out of scope
- Writing raw SQL queries unless strictly required.
- Making domain-level business decisions unrelated to persistence.
- Performance tuning outside of database-level concerns.
- Infrastructure decisions (backups, replicas, hosting).

## Available tools
- PostgreSQL MCP server (read-only): inspect schema, indexes, constraints, and query plans.
- EF Core migrations and model snapshots in the repository.

## Example scenarios
- Adding a new nullable column required by a feature rollout.
- Deciding whether a slow endpoint needs an index or a domain refactor.
- Reviewing a migration that drops or renames a column.