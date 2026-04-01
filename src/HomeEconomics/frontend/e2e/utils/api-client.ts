const API_BASE = "http://localhost:5050/api";

type MovementTypeInput = "income" | "expense";
type MovementFrequencyInput = "none" | "monthly" | "yearly" | "custom";

type ApiMovement = {
  id: number;
  name: string;
  type: number;
};

type ApiMovementListResponse = {
  movements: ApiMovement[];
};

type ApiMovementMonth = {
  id: number;
  year: number;
  month: number;
  monthMovements: Array<{ id: number }>;
};

type YearMonth = {
  year: number;
  month: number;
};

function mapMovementType(type: MovementTypeInput): number {
  return type === "income" ? 0 : 1;
}

function mapFrequency(frequency: MovementFrequencyInput): { type: number; month: number; months: boolean[] } {
  if (frequency === "none") {
    return {
      type: 0,
      month: 0,
      months: new Array<boolean>(12).fill(false),
    };
  }
  
  if (frequency === "monthly") {
    return {
      type: 1,
      month: 0,
      months: new Array<boolean>(12).fill(false),
    };
  }

  if (frequency === "yearly") {
    return {
      type: 2,
      month: 5,
      months: new Array<boolean>(12).fill(false),
    };
  }

  if (frequency === "custom") {
    const months = new Array<boolean>(12).fill(false);
    for (let monthIndex = 0; monthIndex < 12; monthIndex += 3) {
      months[monthIndex] = true;
    }
    return {
      type: 3,
      month: 0,
      months: months,
    };
  }

  throw new Error(`Invalid frequency: ${frequency}`);
}

function isObject(value: unknown): value is Record<string, unknown> {
  return typeof value === "object" && value !== null;
}

function isApiMovementMonth(value: unknown): value is ApiMovementMonth {
  if (!isObject(value)) {
    return false;
  }

  return (
    typeof value.id === "number" &&
    typeof value.year === "number" &&
    typeof value.month === "number" &&
    Array.isArray(value.monthMovements)
  );
}

async function parseJson<T>(response: Response): Promise<T> {
  return (await response.json()) as T;
}

export class TestApiClient {
  private movementCache = new Map<number, ApiMovement>();
  private movementMonthCache = new Map<number, YearMonth>();

  private async cacheMovementMonthFromResponse(response: Response): Promise<void> {
    const clonedResponse = response.clone();
    const payload = await parseJson<unknown>(clonedResponse);
    if (isApiMovementMonth(payload)) {
      this.movementMonthCache.set(payload.id, { year: payload.year, month: payload.month });
    }
  }

  private async getMovementById(id: number): Promise<ApiMovement> {
    const cached = this.movementCache.get(id);
    if (cached) {
      return cached;
    }

    const response = await fetch(`${API_BASE}/movements`);
    if (!response.ok) {
      const errorText = await response.text();
      throw new Error(
        `List movements failed: ${response.status} ${response.statusText} - ${errorText}`
      );
    }

    const data = await parseJson<ApiMovementListResponse>(response);
    const movement = data.movements.find((item) => item.id === id);
    if (!movement) {
      throw new Error(`Movement not found: ${id}`);
    }

    this.movementCache.set(movement.id, movement);
    return movement;
  }

  async createMovement(data: {
    name: string;
    amount: number;
    type: MovementTypeInput;
    frequency: MovementFrequencyInput;
  }): Promise<{ id: number }> {
    const response = await fetch(`${API_BASE}/movements`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({
        name: data.name,
        amount: data.amount,
        type: mapMovementType(data.type),
        frequency: mapFrequency(data.frequency),
      }),
    });

    if (!response.ok) {
      const errorText = await response.text();
      throw new Error(
        `Create movement failed: ${response.status} ${response.statusText} - ${errorText}`
      );
  }

    const id = await parseJson<number>(response);
    this.movementCache.set(id, {
      id,
      name: data.name,
      type: mapMovementType(data.type),
    });

    return { id };
  }

  async deleteMovement(id: number): Promise<void> {
    const response = await fetch(`${API_BASE}/movements/${id}`, {
      method: "DELETE",
    });
    if (!response.ok && response.status !== 404) {
      const errorText = await response.text();
      throw new Error(
        `Delete movement failed: ${response.status} ${response.statusText} - ${errorText}`
      );
    }
    this.movementCache.delete(id);
  }

  async createMovementMonth(year: number, month: number): Promise<{ id: number }> {
    const response = await fetch(`${API_BASE}/movement-months`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ year, month }),
    });
    if (!response.ok) {
      const errorText = await response.text();
      throw new Error(
        `Create month failed: ${response.status} ${response.statusText} - ${errorText}`
      );
    }

    const payload = await parseJson<unknown>(response);
    if (typeof payload === "number") {
      this.movementMonthCache.set(payload, { year, month });
      return { id: payload };
    }

    if (isApiMovementMonth(payload)) {
      this.movementMonthCache.set(payload.id, { year: payload.year, month: payload.month });
      return { id: payload.id };
    }

    throw new Error("Create month failed: invalid response payload");
  }

  async getMovementMonth(year: number, month: number): Promise<ApiMovementMonth | null> {
    const response = await fetch(`${API_BASE}/movement-months/${year}/${month}`);
    if (!response.ok && response.status !== 404) {
      const errorText = await response.text();
      throw new Error(
        `Get month failed: ${response.status} ${response.statusText} - ${errorText}`
      );
    }
    if (response.status === 404) {
      return null;
    }

    const payload = await parseJson<unknown>(response);
    if (!isApiMovementMonth(payload)) {
      throw new Error("Get month failed: invalid response payload");
    }

    this.movementMonthCache.set(payload.id, { year: payload.year, month: payload.month });
    return payload;
  }

  async addMonthMovement(monthId: number, data: { movementId: number; amount: number }): Promise<{ id: number }> {
    const movement = await this.getMovementById(data.movementId);
    const response = await fetch(`${API_BASE}/movement-months/${monthId}/month-movements`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({
        movementMonthId: monthId,
        name: movement.name,
        amount: data.amount,
        type: movement.type,
      }),
    });
    if (!response.ok) {
      const errorText = await response.text();
      throw new Error(
        `Add month movement failed: ${response.status} ${response.statusText} - ${errorText}`
      );
    }

    const payload = await parseJson<unknown>(response);
    if (!isApiMovementMonth(payload)) {
      throw new Error("Add month movement failed: invalid response payload");
    }

    this.movementMonthCache.set(payload.id, { year: payload.year, month: payload.month });
    const createdMovement = payload.monthMovements[payload.monthMovements.length - 1];
    if (!createdMovement || typeof createdMovement.id !== "number") {
      throw new Error("Add month movement failed: created movement id missing");
    }

    return { id: createdMovement.id };
  }

  async deleteMonthMovement(monthId: number, movementId: number): Promise<void> {
    const response = await fetch(`${API_BASE}/movement-months/${monthId}/month-movements/${movementId}`, {
      method: "DELETE",
    });
    if (!response.ok && response.status !== 404) {
      const errorText = await response.text();
      throw new Error(
        `Delete month movement failed: ${response.status} ${response.statusText} - ${errorText}`
      );
    }
  }

  async payMonthMovement(monthId: number, movementId: number): Promise<void> {
    const response = await fetch(`${API_BASE}/movement-months/${monthId}/month-movements/${movementId}/pay`, {
      method: "POST",
    });
    if (!response.ok) {
      const errorText = await response.text();
      throw new Error(
        `Pay failed: ${response.status} ${response.statusText} - ${errorText}`
      );
    }
  }

  async unpayMonthMovement(monthId: number, movementId: number): Promise<void> {
    const response = await fetch(`${API_BASE}/movement-months/${monthId}/month-movements/${movementId}/unpay`, {
      method: "POST",
    });
    if (!response.ok) {
      const errorText = await response.text();
      throw new Error(
        `Unpay failed: ${response.status} ${response.statusText} - ${errorText}`
      );
    }
  }

  async updateMonthMovementAmount(monthId: number, movementId: number, amount: number): Promise<void> {
    const response = await fetch(`${API_BASE}/movement-months/${monthId}/month-movements/${movementId}/update-amount`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ movementMonthId: monthId, monthMovementId: movementId, amount }),
    });
    if (!response.ok) {
      const errorText = await response.text();
      throw new Error(
        `Update amount failed: ${response.status} ${response.statusText} - ${errorText}`
      );
    }
  }

  async moveToNextMonth(monthId: number, movementId: number): Promise<void> {
    const response = await fetch(`${API_BASE}/movement-months/${monthId}/month-movements/${movementId}/to-next-movement-month`, {
      method: "POST",
    });
    if (!response.ok) {
      const errorText = await response.text();
      throw new Error(
        `Move to next month failed: ${response.status} ${response.statusText} - ${errorText}`
      );
    }
  }

  async addStatus(monthId: number, account: number, cash: number): Promise<void> {
    const monthData = this.movementMonthCache.get(monthId);
    if (!monthData) {
      throw new Error(
        `Add status failed: month ${monthId} is not cached. Call createMovementMonth or getMovementMonth first.`,
      );
    }

    const response = await fetch(`${API_BASE}/movement-months/${monthId}/add-status`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({
        year: monthData.year,
        month: monthData.month,
        accountAmount: account,
        cashAmount: cash,
      }),
    });

    if (!response.ok) {
      const errorText = await response.text();
      throw new Error(
        `Add status failed: ${response.status} ${response.statusText} - ${errorText}`
      );
    }

    await this.cacheMovementMonthFromResponse(response);
  }
}

export const apiClient = new TestApiClient();
