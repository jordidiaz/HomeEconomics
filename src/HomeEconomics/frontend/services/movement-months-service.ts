import type { MovementMonth } from "../types/movement-month";

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
}
