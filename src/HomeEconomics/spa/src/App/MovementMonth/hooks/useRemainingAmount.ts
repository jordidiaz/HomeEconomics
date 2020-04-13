import { useEffect, useState } from "react";
import { calculateRemaining } from "../helpers/calcs";
import { TMovementMonth } from "../models/movement-month.models";

export const useRemainingAmount = (movementMonth: TMovementMonth): number => {
  const [remainingAmount, setRemainingAmount] = useState<number>(0);

  useEffect(() => {
    setRemainingAmount(calculateRemaining(movementMonth));
  }, [movementMonth]);

  return remainingAmount;
}