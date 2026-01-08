# database

## Purpose
Guide database and migration decisions for the HomeEconomics backend, focusing on *when* and *why* to change schema, mappings, or persistence behavior.

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

## Guardrails
- Keep migrations minimal and reversible when possible.
- Avoid introducing persistence-specific details into domain entities.
- Prefer explicit, intentional schema evolution over ad-hoc changes.
