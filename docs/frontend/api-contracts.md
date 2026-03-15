
# API Contracts – HomeEconomics API

## Scope
This document defines the **external behavioral contract** of the HomeEconomics API.
It complements the OpenAPI (Swagger) definition and focuses on **guarantees, invariants, and domain behavior**.
Internal implementation details are out of scope.

---

## General Rules

- All monetary values are `decimal` numbers (no floats at domain level).
- IDs are server-generated and immutable.
- Unknown or additional properties are rejected.
- Commands are **intent-based** (CQRS style).
- All errors follow `ProblemDetails` format.

---

## Movement Months

### POST /api/movement-months
Create a new movement month.

**Contract rules**
- `(year, month)` must be unique.
- Month must be between 1 and 12.
- Creating an existing month returns **409 Conflict**.

---

### GET /api/movement-months/{year}/{month}
Retrieve a movement month.

**Contract rules**
- Returns **404** if the month does not exist.
- Returned state is immutable for the request lifecycle.

---

### POST /api/movement-months/{movementMonthId}/month-movements
Add a movement to a month.

**Contract rules**
- `amount` must be non-zero.
- `type` defines sign semantics (income vs expense).
- Month must exist.

---

### POST /api/movement-months/{movementMonthId}/month-movements/{monthMovementId}/pay
Mark a month movement as paid.

**Contract rules**
- Movement must exist.
- Already paid movements return **409 Conflict**.

---

### POST /api/movement-months/{movementMonthId}/month-movements/{monthMovementId}/unpay
Revert payment status.

**Contract rules**
- Only paid movements can be unpaid.
- Invalid transitions return **409 Conflict**.

---

### POST /api/movement-months/{movementMonthId}/month-movements/{monthMovementId}/update-amount
Update movement amount.

**Contract rules**
- Amount must be different from the current value.
- Historical integrity is preserved.

---

### DELETE /api/movement-months/{movementMonthId}/month-movements/{monthMovementId}
Delete a month movement.

**Contract rules**
- Paid movements cannot be deleted.
- Returns **409 Conflict** if deletion violates invariants.

---

### POST /api/movement-months/{movementMonthId}/month-movements/{monthMovementId}/to-next-movement-month
Move movement to the next month.

**Contract rules**
- Target month must exist or be creatable.
- Movement identity is preserved.

---

### POST /api/movement-months/{movementMonthId}/add-status
Add account and cash status snapshot.

**Contract rules**
- One status per `(year, month)`.
- Amounts must be >= 0.

---

## Movements

### POST /api/movements
Create a recurring or one-off movement.

**Contract rules**
- Frequency definition must be valid for its type.
- Monthly frequencies require explicit month selection.

---

### GET /api/movements
List movements.

**Contract rules**
- Order is stable but not guaranteed.
- Deleted movements are excluded.

---

### PUT /api/movements/{id}
Edit a movement.

**Contract rules**
- ID is immutable.
- Frequency changes do not retroactively affect paid months.

---

### DELETE /api/movements/{id}
Delete a movement.

**Contract rules**
- Movements with paid instances cannot be deleted.
- Successful deletion returns **204 No Content**.

---

## Error Model

All errors use `ProblemDetails`:

- `400` → Validation failure
- `404` → Resource not found
- `409` → Domain invariant violation
- `500` → Unexpected error

---

## Versioning

- This contract evolves **additively**.
- Breaking changes require a new API version.

---

**Source of truth**
- OpenAPI defines shape
- `api-contracts.md` defines behavior
