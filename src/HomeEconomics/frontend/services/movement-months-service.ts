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
}
