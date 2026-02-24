let testCounter = 0;

export function generateUniqueTestName(prefix: string): string {
  return `${prefix} ${Date.now()}-${testCounter++}`;
}

export const testMovementDefaults = {
  income: {
    amount: 3000,
    type: "income" as const,
    frequency: "monthly" as const,
  },
  expense: {
    amount: 1200,
    type: "expense" as const,
    frequency: "monthly" as const,
  },
};

export function getCurrentYearMonth(): { year: number; month: number } {
  const now = new Date();
  return {
    year: now.getFullYear(),
    month: now.getMonth() + 1,
  };
}

export function getNextYearMonth(): { year: number; month: number } {
  const now = new Date();
  const next = new Date(now.getFullYear(), now.getMonth() + 1, 1);
  return {
    year: next.getFullYear(),
    month: next.getMonth() + 1,
  };
}
