import { useCallback, useEffect, useMemo, useRef, useState } from "react";
import { MovementMonthsService } from "../services/movement-months-service";

type MovementMonthStatus = {
  pendingTotalExpenses: number;
  pendingTotalIncomes: number;
  accountAmount: number;
  cashAmount: number;
};

type MovementMonthInfo = {
  id: number;
  year: number;
  month: number;
};

type UseMovementMonthStatusFormOptions = {
  movementMonth: MovementMonthInfo | null;
  status: MovementMonthStatus | null;
};

type UseMovementMonthStatusFormResult = {
  accountAmount: string;
  cashAmount: string;
  balance: number;
  submitting: boolean;
  errorMessage: string | null;
  setAccountAmount: (value: string) => void;
  setCashAmount: (value: string) => void;
  submitOnBlur: () => void;
};

const formatAmountInput = (amount: number): string => amount.toFixed(2);

export function useMovementMonthStatusForm(
  options: UseMovementMonthStatusFormOptions,
): UseMovementMonthStatusFormResult {
  const [accountAmount, setAccountAmount] = useState("0.00");
  const [cashAmount, setCashAmount] = useState("0.00");
  const [submitting, setSubmitting] = useState(false);
  const [errorMessage, setErrorMessage] = useState<string | null>(null);
  const lastSubmittedRef = useRef<{ accountAmount: string; cashAmount: string } | null>(
    null,
  );
  const isMounted = useRef(true);

  useEffect(() => {
    isMounted.current = true;
    return () => {
      isMounted.current = false;
    };
  }, []);

  useEffect(() => {
    if (!options.status) {
      return;
    }
    const nextAccount = formatAmountInput(options.status.accountAmount);
    const nextCash = formatAmountInput(options.status.cashAmount);
    setAccountAmount(nextAccount);
    setCashAmount(nextCash);
    lastSubmittedRef.current = { accountAmount: nextAccount, cashAmount: nextCash };
  }, [options.status]);

  const balance = useMemo(() => {
    const account = Number(accountAmount);
    const cash = Number(cashAmount);
    const pendingExpenses = options.status?.pendingTotalExpenses ?? 0;
    const pendingIncomes = options.status?.pendingTotalIncomes ?? 0;
    if (!Number.isFinite(account) || !Number.isFinite(cash)) {
      return 0;
    }
    return account + cash - (pendingExpenses - pendingIncomes);
  }, [accountAmount, cashAmount, options.status]);

  const submitOnBlur = useCallback(async () => {
    if (!options.movementMonth || !options.status) {
      return;
    }

    const parsedAccount = Number(accountAmount);
    const parsedCash = Number(cashAmount);
    if (
      !Number.isFinite(parsedAccount) ||
      !Number.isFinite(parsedCash) ||
      parsedAccount < 0 ||
      parsedCash < 0
    ) {
      setErrorMessage("Introduce valores numericos validos.");
      return;
    }

    setErrorMessage(null);

    const currentValues = { accountAmount, cashAmount };
    const lastSubmitted = lastSubmittedRef.current;
    if (
      lastSubmitted &&
      lastSubmitted.accountAmount === currentValues.accountAmount &&
      lastSubmitted.cashAmount === currentValues.cashAmount
    ) {
      return;
    }

    setSubmitting(true);
    try {
      await MovementMonthsService.addStatus(options.movementMonth.id, {
        year: options.movementMonth.year,
        month: options.movementMonth.month,
        accountAmount: parsedAccount,
        cashAmount: parsedCash,
      });
      if (isMounted.current) {
        lastSubmittedRef.current = currentValues;
      }
    } catch (error) {
      if (isMounted.current) {
        setErrorMessage(
          "No se pudo actualizar el estado del mes. Por favor, inténtalo de nuevo.",
        );
      }
    } finally {
      if (isMounted.current) {
        setSubmitting(false);
      }
    }
  }, [accountAmount, cashAmount, options.movementMonth, options.status]);

  return {
    accountAmount,
    cashAmount,
    balance,
    submitting,
    errorMessage,
    setAccountAmount,
    setCashAmount,
    submitOnBlur,
  };
}
