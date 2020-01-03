import { TMovement } from '../Movement/models/movement.models';
import http from '../../infrastructure/http';

type GetAllMovementsResponse = {
  movements: TMovement[];
}

interface createMovementDTO {
  name: string;
  amount: number;
  type: number;
  frequency: {
    type: number;
    month: number;
    months: boolean[];
  }
}

interface editMovementDTO extends createMovementDTO {
  id: number;
}

const getAll = async (): Promise<TMovement[]> => {
  const response: GetAllMovementsResponse = await http.get<GetAllMovementsResponse>('movements');
  return response.movements;
};

const remove = async (movement: TMovement): Promise<void> => {
  await http.del(`movements/${movement.id}`);
  return;
};

const create = async (movement: TMovement): Promise<number> => {
  const createMovementDTO: createMovementDTO = {
    name: movement.name,
    amount: movement.amount,
    type: movement.type,
    frequency: {
      type: movement.frequencyType,
      month: movement.frequencyMonth,
      months: movement.frequencyMonths,
    }
  };
  return await http.post('movements', createMovementDTO);
};

const edit = async (movement: TMovement): Promise<void> => {
  const editMovementDTO: editMovementDTO = {
    id: movement.id,
    name: movement.name,
    amount: movement.amount,
    type: movement.type,
    frequency: {
      type: movement.frequencyType,
      month: movement.frequencyMonth,
      months: movement.frequencyMonths,
    }
  };
  await http.put(`movements/${movement.id}`, editMovementDTO);
  return;
};

const movementsService = {
  getAll,
  remove,
  create,
  edit
};

export default movementsService;
