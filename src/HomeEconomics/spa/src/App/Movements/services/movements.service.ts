import { TMovement } from '../Movement/models/movement.models';
import http from '../../infrastructure/http';

type GetAllMovementsResponse = {
  movements: TMovement[];
}

const getAllMovements = async (): Promise<TMovement[]> => {
  const response: GetAllMovementsResponse = await http.get<GetAllMovementsResponse>('movements');
  return response.movements;
};

const deleteMovement = async (movement: TMovement): Promise<void> => {
  await http.del(`movements/${movement.id}`);
  return;
};

const movementsService = {
  getAllMovements,
  deleteMovement
};

export default movementsService;
