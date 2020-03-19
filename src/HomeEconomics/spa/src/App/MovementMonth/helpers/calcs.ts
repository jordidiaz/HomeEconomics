import { TMovementMonth } from "../models/movement-month.models";

export const calculateRemaining = (movementMonth: TMovementMonth | undefined): number => {
  if (!movementMonth) {
    return 0;
  }

  let accountAmount = 0;
  let cashAmount = 0;

  if (!isNaN(movementMonth.status.accountAmount)) {
    accountAmount = movementMonth.status.accountAmount;
  }

  if (!isNaN(movementMonth.status.cashAmount)) {
    cashAmount = movementMonth.status.cashAmount;
  }

  const remaining = (accountAmount + cashAmount) - (movementMonth.status.pendingTotalExpenses - movementMonth.status.pendingTotalIncomes);
  return parseFloat(remaining.toFixed(2));
};
