---
name: dotnet-api
description: Guide backend changes in the .NET API using CQRS and Clean Architecture, focusing on focusing on architectural and layering decisions over rote steps over rote steps
---

## When to use this skill
- You are adding or modifying API behavior in `src/HomeEconomics/Features/`.
- You are changing domain behavior in `src/Domain/` that affects commands/queries.
- You need to decide where new logic belongs (domain vs. feature handler vs. persistence).

## Core decision rules

### 1) Choose the right layer for new behavior
- **Domain layer (`src/Domain/`)**: Put business rules that must be true regardless of transport or storage (e.g., validation rules for Movement recurrence). If the rule should be enforced even without an API call, it belongs here.
- **Feature handlers (`src/HomeEconomics/Features/`)**: Put orchestration logic that depends on application concerns (e.g., coordinating multiple aggregates, translating requests into domain changes). If the logic is tied to a request/response shape, it belongs here.
- **Persistence (`src/Persistence/`)**: Only data access concerns (query shape, mapping, EF configuration). If logic only exists to optimize queries or map data, keep it here.

### 2) Decide between Command vs Query
- **Command** when you change state or trigger side effects (create/update/delete, marking paid). Commands should return minimal data needed by the caller.
- **Query** when you read state only (listing movement months, reporting summaries). Queries should not modify domain entities or invoke persistence changes.

### 3) Decide how to structure a feature
- Prefer **feature folders** scoped to a domain concept (e.g., `Movements`, `MovementMonths`) rather than technical grouping.
- If a new behavior is a variant of an existing feature (e.g., another way to list movements), extend the existing feature folder rather than creating a new top-level feature.
- If the behavior introduces a distinct domain concept, create a new feature folder aligned with the new concept.

### 4) Decide where validation belongs
- **Domain invariants** (must always be true) belong in domain entities/value objects.
- **Request validation** (input shape/range from API) belongs in the feature layer; use validators there and keep domain rules consistent.
- Avoid duplicating the same rule in both layers; prefer domain for invariant rules and feature-level for request shape.

### 5) Decide error handling strategy
- If an error represents invalid domain state, raise it from the domain layer.
- If it represents invalid request data, handle it in the feature layer.
- Favor clear, deterministic failure paths over silent no-ops; align with CQRS expectations that commands should fail fast on invalid state.

### 6) Decide how to evolve the domain model
- If a change affects multiple features, update the **domain model first** and then adapt handlers.
- If a change is localized to a single API behavior without changing core invariants, keep it within the feature layer.

## Guardrails
- Keep CQRS separation: no writes in queries, no reads with side effects in commands.
- Prefer explicit domain methods over mutating properties directly in feature handlers.
- Avoid leaking persistence concerns into domain or feature decisions.

## Out of scope
- Database schema or migration decisions (handled by the database skill).
- Infrastructure or deployment concerns.
- Frontend or API consumer decisions.
- Cross-service integration patterns.

## Available context
- Feature handlers under src/HomeEconomics/Features/.
- Domain entities and value objects under src/Domain/.
- EF Core mappings under src/Persistence/.

## Example scenarios
- Adding a new command that creates a Movement with new domain rules.
- Refactoring logic currently inside a handler into a domain entity.
- Deciding whether a new endpoint should be a query or command.


