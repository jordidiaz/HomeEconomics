import type { MonthMovement } from "./month-movement";

export type MovementMonth = {
  id: number;
  year: number;
  month: number;
  nextMovementMonthExists: boolean;
  previousMovementMonthExists: boolean;
  status: {
    pendingTotalExpenses: number;
    pendingTotalIncomes: number;
    accountAmount: number;
    cashAmount: number;
  };
  monthMovements: MonthMovement[];
};
