import { useEffect, useState } from 'react';
import movementMonthService from '../services/movement-months.service';
import { TMovementMonth } from '../models/movement-month.models';

type UseMovementMonth = {
  movementMonth: TMovementMonth | undefined;
  setMovementMonth: React.Dispatch<React.SetStateAction<TMovementMonth | undefined>>;
}

export const useMovementMonth = (year: number, month: number): UseMovementMonth => {

  const [movementMonth, setMovementMonth] = useState<TMovementMonth>();

  useEffect(() => {
    async function getMovementMonth(): Promise<void> {
      const movementMonth = await movementMonthService.get(year, month);
      setMovementMonth(movementMonth);
    }
    getMovementMonth();
  }, [month, year]);

  return {
    movementMonth,
    setMovementMonth
  }
}