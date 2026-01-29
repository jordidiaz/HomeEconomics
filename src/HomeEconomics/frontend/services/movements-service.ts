import { FrequencyType } from "../types/frequency-type";
import type { Movement } from "../types/movement";
import { MovementType } from "../types/movement-type";

type MovementsResponse = {
  movements: Movement[];
};

export type CreateMovementRequest = {
  name: string;
  amount: number;
  type: MovementType;
  frequency: {
    type: FrequencyType;
    month: number;
    months: boolean[];
  };
};

export type UpdateMovementRequest = CreateMovementRequest & { id: number };

export class MovementsService {
  static async getAll(): Promise<Movement[]> {
    const response = await fetch("/api/movements");

    if (!response.ok) {
      throw new Error(`Failed to fetch movements (${response.status})`);
    }

    const data: MovementsResponse = await response.json();
    return data.movements;
  }

  static async create(request: CreateMovementRequest): Promise<number> {
    const response = await fetch("/api/movements", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(request),
    });

    if (!response.ok) {
      throw new Error(`Failed to create movement (${response.status})`);
    }

    const data: number = await response.json();
    return data;
  }

  static async delete(id: number): Promise<void> {
    const response = await fetch(`/api/movements/${id}`, {
      method: "DELETE",
    });

    if (!response.ok) {
      throw new Error(`Failed to delete movement (${response.status})`);
    }
  }

  static async update(id: number, request: UpdateMovementRequest): Promise<void> {
    request.id = id;
    const response = await fetch(`/api/movements/${id}`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(request),
    });

    if (!response.ok) {
      throw new Error(`Failed to update movement (${response.status})`);
    }
  }
}
