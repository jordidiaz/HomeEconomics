import type { MovementMonth } from "../types/movement-month";

export class MovementMonthsService {
  static async getByYearMonth(year: number, month: number): Promise<MovementMonth> {
    const response = await fetch(`/api/movement-months/${year}/${month}`);

    if (!response.ok) {
      throw new Error(`Failed to fetch movement month (${response.status})`);
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
      throw new Error(`Failed to pay month movement (${response.status})`);
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
      throw new Error(`Failed to unpay month movement (${response.status})`);
    }
  }
}
