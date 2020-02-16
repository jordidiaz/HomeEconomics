import { TMovementMonth } from "../../App/MovementMonth/models/movement-month.models";
import { MovementType } from "../../App/Movements/Movement/models/movement.models";

export const getMovementMonth = (): TMovementMonth => {
  return {
    id: 1,
    year: 2020,
    month: 3,
    monthMovements: [
      {
        id: 1,
        name: 'Gasolina',
        type: MovementType.Expense,
        amount: 60
      },
      {
        id: 2,
        name: 'Jangela',
        type: MovementType.Expense,
        amount: 72
      }
    ]
  };
};