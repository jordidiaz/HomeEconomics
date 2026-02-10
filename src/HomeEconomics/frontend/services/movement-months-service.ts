import type { MovementMonth } from "../types/movement-month";
import type { MovementType } from "../types/movement-type";

export class MovementMonthsService {
  private static createError(message: string, status: number): Error & { status: number } {
    return Object.assign(new Error(message), { status });
  }

  static async create(year: number, month: number): Promise<MovementMonth> {
    const response = await fetch("/api/movement-months", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({ year, month }),
    });

    if (!response.ok) {
      throw MovementMonthsService.createError(
        `Failed to create movement month (${response.status})`,
        response.status,
      );
    }

    const data: MovementMonth = await response.json();
    return data;
  }

  static async getByYearMonth(year: number, month: number): Promise<MovementMonth> {
    const response = await fetch(`/api/movement-months/${year}/${month}`);

    if (!response.ok) {
      throw MovementMonthsService.createError(
        `Failed to fetch movement month (${response.status})`,
        response.status,
      );
    }

    const data: MovementMonth = await response.json();
    return data;
  }

  static async addMonthMovement(
    movementMonthId: number,
    movement: { movementMonthId: number, name: string; amount: number; type: MovementType },
  ): Promise<MovementMonth> {
    movement.movementMonthId = movementMonthId;
    const response = await fetch(`/api/movement-months/${movementMonthId}/month-movements`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(movement),
    });

    if (!response.ok) {
      throw MovementMonthsService.createError(
        `Failed to add month movement (${response.status})`,
        response.status,
      );
    }

    const data: MovementMonth = await response.json();
    return data;
  }

  static async payMonthMovement(
    movementMonthId: number,
    monthMovementId: number,
  ): Promise<void> {
    const response = await fetch(
      `/api/movement-months/${movementMonthId}/month-movements/${monthMovementId}/pay`,
      {
        method: "POST",
      },
    );

    if (!response.ok) {
      throw MovementMonthsService.createError(
        `Failed to pay month movement (${response.status})`,
        response.status,
      );
    }
  }

  static async unpayMonthMovement(
    movementMonthId: number,
    monthMovementId: number,
  ): Promise<void> {
    const response = await fetch(
      `/api/movement-months/${movementMonthId}/month-movements/${monthMovementId}/unpay`,
      {
        method: "POST",
      },
    );

    if (!response.ok) {
      throw MovementMonthsService.createError(
        `Failed to unpay month movement (${response.status})`,
        response.status,
      );
    }
  }

  static async updateMonthMovementAmount(
    movementMonthId: number,
    monthMovementId: number,
    amount: number,
  ): Promise<void> {
    const response = await fetch(
      `/api/movement-months/${movementMonthId}/month-movements/${monthMovementId}/update-amount`,
      {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          movementMonthId,
          monthMovementId,
          amount,
        }),
      },
    );

    if (!response.ok) {
      throw MovementMonthsService.createError(
        `Failed to update month movement amount (${response.status})`,
        response.status,
      );
    }
  }

  static async deleteMonthMovement(
    movementMonthId: number,
    monthMovementId: number,
  ): Promise<void> {
    const response = await fetch(
      `/api/movement-months/${movementMonthId}/month-movements/${monthMovementId}`,
      {
        method: "DELETE",
      },
    );

    if (!response.ok) {
      throw MovementMonthsService.createError(
        `Failed to delete month movement (${response.status})`,
        response.status,
      );
    }
  }

  static async moveMonthMovementToNextMonth(
    movementMonthId: number,
    monthMovementId: number,
  ): Promise<void> {
    const response = await fetch(
      `/api/movement-months/${movementMonthId}/month-movements/${monthMovementId}/to-next-movement-month`,
      {
        method: "POST",
      },
    );

    if (!response.ok) {
      throw MovementMonthsService.createError(
        `Failed to move month movement to next month (${response.status})`,
        response.status,
      );
    }
  }

  static async addStatus(
    movementMonthId: number,
    status: {
      year: number;
      month: number;
      accountAmount: number;
      cashAmount: number;
    },
  ): Promise<void> {
    const response = await fetch(`/api/movement-months/${movementMonthId}/add-status`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(status),
    });

    if (!response.ok) {
      throw MovementMonthsService.createError(
        `Failed to add movement month status (${response.status})`,
        response.status,
      );
    }
  }
}
