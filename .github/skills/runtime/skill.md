---
name: runtime
description: Provide decision-making guidance for configuring and reasoning about the app's runtime environment in development and deployment contexts
---

## When to use this skill
- You need to decide how a change impacts runtime configuration or deployment behavior.
- You are troubleshooting environment-specific behavior (ports, services, or data access).
- You are deciding whether a change should be environment-specific or universal.

## Core decision rules

### 1) Decide whether a change is environment-specific
- If the behavior differs across local, CI, and production, treat it as **environment-specific** and isolate it with configuration.
- If the behavior should be consistent everywhere, avoid environment-specific branches and keep it uniform.

### 2) Decide how to manage configuration
- Use configuration for values that vary by environment (connection strings, ports, feature toggles).
- Avoid hardcoding values that might differ across environments; prefer explicit configuration for clarity.

### 3) Decide how to scope runtime dependencies
- If a dependency is required for core functionality, ensure it is available in all target environments.
- If it is optional or developer-only, keep it isolated from production requirements.

### 4) Decide how to handle service connectivity issues
- If a failure is due to missing external services, surface a clear configuration error rather than masking the issue.
- If the failure is transient, design for retry or graceful degradation when appropriate.

### 5) Decide on runtime defaults
- Choose safe defaults that allow local development to start quickly.
- Avoid defaults that may cause unintended production behavior.

## Guardrails
- Keep environment behavior explicit and documented in configuration.
- Avoid coupling runtime setup logic to domain behavior.
