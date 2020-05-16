import { useEffect, useState } from "react";
import { calculateRemaining } from "../helpers/calcs";
import { TMovementMonth } from "../models/movement-month.models";
import { MonthStatusFormValues } from "../components/MonthStatus/MonthStatus";

export const useAmounts = (movementMonth: TMovementMonth, setAmounts: React.Dispatch<React.SetStateAction<MonthStatusFormValues>>): number => {
  const [remainingAmount, setRemainingAmount] = useState<number>(0);

  useEffect(() => {
    setRemainingAmount(calculateRemaining(movementMonth));
    setAmounts({
      accountAmount: movementMonth.status.accountAmount.toString(),
      cashAmount: movementMonth.status.cashAmount.toString(),
    })
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [movementMonth]);

  return remainingAmount;
}