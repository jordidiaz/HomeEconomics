import { useEffect } from 'react';
import { TMovement } from '../models/movement.models';
import { MovementFormValues } from '../components/MovementForm/MovementForm';

const createValuesFromTMovement = (movement: TMovement): MovementFormValues => {
  const values: MovementFormValues = {
    id: movement.id,
    name: movement.name,
    amount: movement.amount.toString(),
    type: movement.type,
    frequencyType: movement.frequencyType,
    frequencyMonth: movement.frequencyMonth.toString(),
    frequencyMonths: movement.frequencyMonths
  }

  return values;
}

export const useMovement = (movement: TMovement, setMovement: React.Dispatch<React.SetStateAction<MovementFormValues>>): void => {
  useEffect(() => {
    setMovement(createValuesFromTMovement(movement))
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [movement]);
}