import type { MovementType } from "./movement-type";

export type MonthMovement = {
  id: number;
  name: string;
  amount: number;
  type: MovementType;
  paid: boolean;
};
