import { useCallback, useMemo, useState } from "react";
import { FrequencyType } from "../types/frequency-type";
import { MovementType } from "../types/movement-type";
import { MovementsService } from "../services/movements-service";
import type { CreateMovementRequest } from "../services/movements-service";

type UseCreateMovementOptions = {
  onCreated?: () => Promise<void> | void;
};

type UseCreateMovementResult = {
  name: string;
  amount: string;
  type: MovementType;
  frequencyType: FrequencyType;
  frequencyMonth: number;
  customMonths: number[];
  submitting: boolean;
  errorMessage: string | null;
  validationMessage: string | null;
  setName: (value: string) => void;
  setAmount: (value: string) => void;
  setType: (value: MovementType) => void;
  setFrequencyType: (value: FrequencyType) => void;
  setFrequencyMonth: (value: number) => void;
  setCustomMonths: (value: number[]) => void;
  submit: () => Promise<void>;
};

const monthValues = [
  1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
] as const;

const emptyMonths = monthValues.map(() => false);

const isValidMonth = (month: number): boolean => month >= 1 && month <= 12;

const buildFrequencyMonths = (customMonths: number[]): boolean[] =>
  monthValues.map((month) => customMonths.includes(month));

export function useCreateMovement(
  options: UseCreateMovementOptions = {},
): UseCreateMovementResult {
  const [name, setName] = useState("");
  const [amount, setAmount] = useState("");
  const [type, setType] = useState<MovementType>(MovementType.Undefined);
  const [frequencyType, setFrequencyType] = useState<FrequencyType>(
    FrequencyType.Undefined,
  );
  const [frequencyMonth, setFrequencyMonth] = useState(1);
  const [customMonths, setCustomMonths] = useState<number[]>([]);
  const [submitting, setSubmitting] = useState(false);
  const [errorMessage, setErrorMessage] = useState<string | null>(null);
  const [validationMessage, setValidationMessage] = useState<string | null>(null);

  const resetForm = useCallback(() => {
    setName("");
    setAmount("");
    setType(MovementType.Undefined);
    setFrequencyType(FrequencyType.Undefined);
    setFrequencyMonth(1);
    setCustomMonths([]);
    setErrorMessage(null);
    setValidationMessage(null);
  }, []);

  const isValid = useMemo(() => {
    const trimmedName = name.trim();
    const parsedAmount = Number(amount);
    if (!trimmedName) {
      return false;
    }
    if (!Number.isFinite(parsedAmount) || parsedAmount < 0) {
      return false;
    }
    if (type === MovementType.Undefined) {
      return false;
    }
    if (frequencyType === FrequencyType.Undefined) {
      return false;
    }
    if (frequencyType === FrequencyType.Yearly && !isValidMonth(frequencyMonth)) {
      return false;
    }
    if (frequencyType === FrequencyType.Custom && customMonths.length < 2) {
      return false;
    }
    return true;
  }, [amount, customMonths.length, frequencyMonth, frequencyType, name, type]);

  const submit = useCallback(async () => {
    setValidationMessage(null);
    setErrorMessage(null);

    if (!isValid) {
      setValidationMessage("Completa los campos requeridos.");
      return;
    }

    const parsedAmount = Number(amount);
    const trimmedName = name.trim();
    const frequencyMonths =
      frequencyType === FrequencyType.Custom
        ? buildFrequencyMonths(customMonths)
        : emptyMonths;
    const request: CreateMovementRequest = {
      name: trimmedName,
      amount: parsedAmount,
      type,
      frequency: {
        type: frequencyType,
        month: frequencyType === FrequencyType.Yearly ? frequencyMonth : 0,
        months: frequencyMonths,
      },
    };

    setSubmitting(true);
    try {
      await MovementsService.create(request);
      resetForm();
      if (options.onCreated) {
        await options.onCreated();
      }
    } catch (error) {
      setErrorMessage(
        "No se pudo crear el movimiento. Por favor, inténtalo de nuevo.",
      );
    } finally {
      setSubmitting(false);
    }
  }, [
    amount,
    customMonths,
    frequencyMonth,
    frequencyType,
    isValid,
    name,
    options,
    resetForm,
    type,
  ]);

  const handleFrequencyTypeChange = useCallback((value: FrequencyType) => {
    setFrequencyType(value);
    setFrequencyMonth(1);
    setCustomMonths([]);
  }, []);

  const handleCustomMonthsChange = useCallback((value: number[]) => {
    const uniqueMonths = Array.from(new Set(value)).sort((a, b) => a - b);
    setCustomMonths(uniqueMonths);
  }, []);

  return {
    name,
    amount,
    type,
    frequencyType,
    frequencyMonth,
    customMonths,
    submitting,
    errorMessage,
    validationMessage,
    setName,
    setAmount,
    setType,
    setFrequencyType: handleFrequencyTypeChange,
    setFrequencyMonth,
    setCustomMonths: handleCustomMonthsChange,
    submit,
  };
}
