# API

## Base URL
The API is served under the `/api` prefix.

- **Backend**: `https://localhost:5001/api` (or `http://localhost:5000/api` for local development).
- **SPA**: the base is configured with the `REACT_APP_API_BASE_URL` variable and `/api/` is then appended in the client.

## Interactive documentation
- **Swagger UI**: `/swagger`
- **OpenAPI document**: `/swagger/hm/swagger.json`

## Errors and validation
- **400 Bad Request**: validation errors (FluentValidation) with `ValidationProblemDetails`.
- **404 Not Found**: resource not found (for example, missing monthly detail).
- **409 Conflict**: business conflicts (for example, trying to create a duplicate movement).

## Endpoints

### Movements

#### GET `/movements`
Returns the list of movements.

**200 Response**
```json
{
  "movements": [
    {
      "id": 1,
      "name": "Renta",
      "amount": 900,
      "type": 1,
      "frequencyType": 1,
      "frequencyMonths": [true, true, true, true, true, true, true, true, true, true, true, true],
      "frequencyMonth": 0
    }
  ]
}
```

#### POST `/movements`
Creates a movement.

**Body**
```json
{
  "name": "Renta",
  "amount": 900,
  "type": 1,
  "frequency": {
    "type": 1,
    "month": 0,
    "months": [true, true, true, true, true, true, true, true, true, true, true, true]
  }
}
```

**200 Response**
```json
1
```

#### PUT `/movements/{id}`
Edits an existing movement.

**Body**
```json
{
  "id": 1,
  "name": "Renta",
  "amount": 950,
  "type": 1,
  "frequency": {
    "type": 1,
    "month": 0,
    "months": [true, true, true, true, true, true, true, true, true, true, true, true]
  }
}
```

**204 Response** (no content)

#### DELETE `/movements/{id}`
Deletes a movement.

**204 Response** (no content)

**Relevant enums**
- `type` (`MovementType`): `0 = Income`, `1 = Expense`.
- `frequency.type` (`FrequencyType`): `0 = None`, `1 = Monthly`, `2 = Yearly`, `3 = Custom`.

---

### Movement Months

#### GET `/movement-months/{year}/{month}`
Returns the month detail with movements and status.

**200 Response**
```json
{
  "id": 4,
  "year": 2024,
  "month": 10,
  "nextMovementMonthExists": true,
  "previousMovementMonthExists": false,
  "status": {
    "pendingTotalExpenses": 450,
    "pendingTotalIncomes": 1200,
    "accountAmount": 300,
    "cashAmount": 50
  },
  "monthMovements": [
    {
      "id": 10,
      "name": "Renta",
      "amount": 900,
      "type": 1,
      "paid": false
    }
  ]
}
```

**404 Response** if the month does not exist.

#### POST `/movement-months`
Creates a month with movements based on frequency.

**Body**
```json
{
  "year": 2024,
  "month": 10
}
```

**200 Response**: `MovementMonthResponse` (same as detail).

#### POST `/movement-months/{movementMonthId}/month-movements`
Adds a movement to the month.

**Body**
```json
{
  "movementMonthId": 4,
  "name": "Compra",
  "amount": 100,
  "type": 1
}
```

**200 Response**: `MovementMonthResponse`.

#### POST `/movement-months/{movementMonthId}/month-movements/{monthMovementId}/update-amount`
Updates the monthly movement amount.

**Body**
```json
{
  "movementMonthId": 4,
  "monthMovementId": 10,
  "amount": 120
}
```

**200 Response**: `MovementMonthResponse`.

#### DELETE `/movement-months/{movementMonthId}/month-movements/{monthMovementId}`
Deletes a month movement.

**200 Response**: `MovementMonthResponse`.

#### POST `/movement-months/{movementMonthId}/month-movements/{monthMovementId}/pay`
Marks a movement as paid.

**200 Response**: `MovementMonthResponse`.

#### POST `/movement-months/{movementMonthId}/month-movements/{monthMovementId}/unpay`
Marks a movement as unpaid.

**200 Response**: `MovementMonthResponse`.

#### POST `/movement-months/{movementMonthId}/month-movements/{monthMovementId}/to-next-movement-month`
Moves a movement to the next month (if it exists).

**200 Response**: `MovementMonthResponse`.

#### POST `/movement-months/{movementMonthId}/add-status`
Adds a status (balances) to the month.

**Body**
```json
{
  "year": 2024,
  "month": 10,
  "accountAmount": 300,
  "cashAmount": 50
}
```

**200 Response**: `MovementMonthResponse`.

---

## Health checks
- `GET /self`: application health check.
- `GET /npgsql`: PostgreSQL health check.
