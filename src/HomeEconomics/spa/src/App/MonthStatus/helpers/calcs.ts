import { TMovementMonth } from "../../MovementMonth/models/movement-month.models";

export const calculateRemaining = (movementMonth: TMovementMonth | undefined, accountAmount: number, cashAmount: number): number => {
  if (!movementMonth) {
    return 0;
  }

  if (isNaN(accountAmount)) {
    accountAmount = 0;
  }

  if (isNaN(cashAmount)) {
    cashAmount = 0;
  }

  const remaining = (accountAmount + cashAmount) - (movementMonth.pendingTotalExpenses - movementMonth.pendingTotalIncomes);
  return parseFloat(remaining.toFixed(2));
};
