import { TMovement } from '../Movement/models/movement.models';

const apiBaseUrl: string = process.env.REACT_APP_API_BASE_URL as string;

type GetAllMovementsResponse = {
  movements: TMovement[];
}

const getAllMovements = (): Promise<TMovement[]> => {
  return fetch(`${apiBaseUrl}/api/movements`)
    .then((response: Response) => response.json())
    .then((result: GetAllMovementsResponse) => result.movements)
    .catch((err) => { throw new Error(err) });
}

const movementsService = {
  getAllMovements
}

export default movementsService;
