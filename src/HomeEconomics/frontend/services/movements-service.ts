import type { Movement } from "../types/movement";

type MovementsResponse = {
  movements: Movement[];
};

export class MovementsService {
  static async getAll(): Promise<Movement[]> {
    const response = await fetch("/api/movements");

    if (!response.ok) {
      throw new Error(`Failed to fetch movements (${response.status})`);
    }

    const data: MovementsResponse = await response.json();
    return data.movements;
  }
}
