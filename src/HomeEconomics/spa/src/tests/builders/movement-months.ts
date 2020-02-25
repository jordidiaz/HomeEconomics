import { TMovementMonth } from "../../App/MovementMonth/models/movement-month.models";
import { MovementType } from "../../App/Movements/Movement/models/movement.models";

export const getMovementMonth = (): TMovementMonth => {
  return {
    id: 1,
    year: 2020,
    month: 3,
    pendingTotalExpenses: 132,
    pendingTotalIncomes: 22,
    monthMovements: [
      {
        id: 1,
        name: 'Gasolina',
        type: MovementType.Expense,
        amount: 60,
        paid: false
      },
      {
        id: 2,
        name: 'Jangela',
        type: MovementType.Expense,
        amount: 72,
        paid: true
      }
    ]
  };
};