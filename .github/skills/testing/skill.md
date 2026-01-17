# testing

## Purpose
Provide decision-making guidance for selecting the right test layer, scope, and quality checks for this repository.

## When to use this skill
- You are adding or changing behavior in the domain, API features, or frontend.
- You need to decide which test layer best validates a change.
- You are weighing test coverage against cost (time, complexity, fragility).

## Core decision rules

### 1) Choose the smallest test layer that proves the behavior
- **Domain/unit** for pure business rules and invariants (fast, localized signal).
- **Application/API** for handler workflows, validation, and persistence boundaries.
- **Integration/end-to-end** for cross-layer behavior that cannot be trusted to lower layers alone.

### 2) Decide when to add new tests
- Add tests for new behavior, changed outcomes, or fragile areas.
- Skip only when the change is strictly mechanical and existing tests already verify the same outcome.

### 3) Decide how to prevent regressions
- If a bug escaped detection, add a test that would have failed before the fix.
- Assert outcomes and externally visible behavior over internal wiring to reduce churn.

### 4) Decide how to balance coverage vs. speed
- Favor fast tests for frequent changes; reserve slower tests for cross-layer risks.
- Avoid duplicating the same assertion across multiple layers unless it mitigates a known risk.

### 5) Decide what quality checks are required
- If a change affects public behavior, ensure tests cover the new behavior path.
- If a change affects data integrity, cover both valid and invalid inputs.
- If a change is user-facing, include coverage that verifies the UX outcome, not just state updates.

## Guardrails
- Tests should express intent and outcomes, not internal wiring.
- Keep the signal clear: one test should validate one behavioral expectation.
