import { useEffect, useState } from 'react';
import { TMovement } from '../models/movement.models';
import movementsService from '../services/movements.service';

type UseMovements = {
  movements: TMovement[];
  setMovements: React.Dispatch<React.SetStateAction<TMovement[]>>;
}

export const useMovements = (): UseMovements => {

  const [movements, setMovements] = useState<TMovement[]>([]);

  useEffect(() => {
    async function getMovements(): Promise<void> {
      setMovements(await movementsService.getAll());
    }
    getMovements();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  return {
    movements,
    setMovements
  }
}