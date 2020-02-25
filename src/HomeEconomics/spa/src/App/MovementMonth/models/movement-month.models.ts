import { MovementType } from "../../Movements/Movement/models/movement.models";

export type TMovementMonth = {
  id: number;
  year: number;
  month: number;
  pendingTotalExpenses: number;
  pendingTotalIncomes: number;
  monthMovements: TMonthMovement[];
}

export type TMonthMovement = {
  id: number;
  name: string;
  amount: number;
  type: MovementType;
  paid: boolean;
}