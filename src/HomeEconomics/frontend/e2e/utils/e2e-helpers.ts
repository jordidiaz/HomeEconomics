import { getCurrentYearMonth, getNextYearMonth } from "../fixtures/test-data";
import { apiClient } from "./api-client";

type MovementMonthRef = {
  id: number;
  year: number;
  month: number;
};

export function inputByTestId(testIdSelector: string): string {
  return `${testIdSelector} input`;
}

export async function getOrCreateMovementMonth(
  year: number,
  month: number,
): Promise<MovementMonthRef> {
  const existing = await apiClient.getMovementMonth(year, month);
  if (existing) {
    return { id: existing.id, year, month };
  }

  try {
    const created = await apiClient.createMovementMonth(year, month);
    return { id: created.id, year, month };
  } catch {
    const afterCreate = await apiClient.getMovementMonth(year, month);
    if (!afterCreate) {
      throw new Error(`Could not create movement month ${year}-${month}`);
    }
    return { id: afterCreate.id, year, month };
  }
}

export async function getOrCreateCurrentMovementMonth(): Promise<MovementMonthRef> {
  const { year, month } = getCurrentYearMonth();
  return getOrCreateMovementMonth(year, month);
}

export async function getOrCreateNextMovementMonth(): Promise<MovementMonthRef> {
  const { year, month } = getNextYearMonth();
  return getOrCreateMovementMonth(year, month);
}
