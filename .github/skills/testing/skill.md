# testing

## Purpose
Provide decision-making guidance on what to test, where to test it, and how to balance coverage with confidence for this repository.

## When to use this skill
- You are adding or changing behavior in the domain, API features, or frontend.
- You need to decide which test layer best validates a change.
- You are evaluating whether a change requires additional checks beyond existing coverage.

## Core decision rules

### 1) Choose the smallest test layer that proves the behavior
- **Domain/unit tests** when logic is pure business rules or invariants.
- **Application/API tests** when behavior spans handlers, validation, or persistence boundaries.
- **Integration/end-to-end tests** when correctness depends on multiple layers working together.

### 2) Decide when to add new tests
- Add tests when behavior is new, non-trivial, or historically brittle.
- Skip new tests only if the change is strictly mechanical and fully covered by existing tests.

### 3) Decide how to handle regressions
- If a bug reached production or was hard to detect, add a test that would have caught it.
- Prefer tests that assert outcomes over implementation details to reduce churn.

### 4) Decide how to balance coverage vs. speed
- Favor fast tests for frequent changes; reserve slower tests for cross-layer risks.
- If a change touches only one layer, avoid broad tests that duplicate the same signal.

### 5) Decide what quality checks are required
- If a change affects public behavior, ensure tests cover the new behavior path.
- If a change affects data integrity, add tests that validate both the happy path and invalid inputs.

## Guardrails
- Tests should express intent and outcomes, not internal wiring.
- Avoid duplicating the same assertion across multiple layers unless it mitigates a known risk.
