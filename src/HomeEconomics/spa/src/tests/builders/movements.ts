import { TMovement, MovementType, FrequencyType } from '../../App/Movements/models/movement.models';

export const getMovements = (quantity: number = 1): TMovement[] => {
  const movements: TMovement[] = [];

  for (let index = 1; index <= quantity; index++) {
    const movement: TMovement = Object.create(baseMovement);
    movement.id = index;
    movement.name = index.toString();
    movement.amount = 1;

    movements.push(movement);
  }

  return movements;
};

const baseMovement: TMovement = {
  id: -1,
  name: '',
  amount: 0,
  type: MovementType.Expense,
  frequencyType: FrequencyType.None,
  frequencyMonth: 0,
  frequencyMonths: []
};