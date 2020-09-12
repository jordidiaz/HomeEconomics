import { MovementType } from "../../Movements/models/movement.models";

export type TMovementMonth = {
  id: number;
  year: number;
  month: number;
  status: TStatus;
  monthMovements: TMonthMovement[];
  nextMovementMonthExists: boolean;
}

export type TStatus = {
  pendingTotalExpenses: number;
  pendingTotalIncomes: number;
  accountAmount: number;
  cashAmount: number;
}

export type TMonthMovement = {
  id: number;
  name: string;
  amount: number;
  type: MovementType;
  paid: boolean;
}