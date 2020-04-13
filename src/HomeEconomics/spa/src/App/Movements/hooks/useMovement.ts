import { useEffect } from 'react';
import { TMovement } from '../models/movement.models';

export const useMovement = (movement: TMovement, setMovement: React.Dispatch<React.SetStateAction<TMovement>>): void => {
  useEffect(() => {
    setMovement(movement)
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [movement]);
}