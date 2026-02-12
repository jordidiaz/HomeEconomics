import { useCallback, useMemo, useState } from "react";
import { FrequencyType } from "../types/frequency-type";
import type { Movement } from "../types/movement";
import { MovementType } from "../types/movement-type";
import {
  MovementsService,
  type CreateMovementRequest,
  type UpdateMovementRequest,
} from "../services/movements-service";

type UseMovementFormOptions = {
  onSaved?: () => Promise<void> | void;
};

type UseMovementFormResult = {
  name: string;
  amount: string;
  type: MovementType;
  frequencyType: FrequencyType;
  frequencyMonth: number;
  customMonths: number[];
  submitting: boolean;
  errorMessage: string | null;
  validationMessage: string | null;
  isEditing: boolean;
  setName: (value: string) => void;
  setAmount: (value: string) => void;
  setType: (value: MovementType) => void;
  setFrequencyType: (value: FrequencyType) => void;
  setFrequencyMonth: (value: number) => void;
  setCustomMonths: (value: number[]) => void;
  submit: () => Promise<void>;
  startEdit: (movement: Movement) => void;
  cancel: () => void;
};

const monthValues = [
  1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
] as const;

const emptyMonths = monthValues.map(() => false);

const isValidMonth = (month: number): boolean => month >= 1 && month <= 12;

const buildFrequencyMonths = (customMonths: number[]): boolean[] =>
  monthValues.map((month) => customMonths.includes(month));

const toCustomMonths = (months: boolean[]): number[] =>
  months
    .map((isSelected, index) => (isSelected ? index + 1 : null))
    .filter((month): month is number => Boolean(month));

const buildRequest = (
  name: string,
  amount: string,
  type: MovementType,
  frequencyType: FrequencyType,
  frequencyMonth: number,
  customMonths: number[],
): CreateMovementRequest => {
  const parsedAmount = Number(amount);
  const trimmedName = name.trim();
  const frequencyMonths =
    frequencyType === FrequencyType.Custom
      ? buildFrequencyMonths(customMonths)
      : emptyMonths;

  return {
    name: trimmedName,
    amount: parsedAmount,
    type,
    frequency: {
      type: frequencyType,
      month: frequencyType === FrequencyType.Yearly ? frequencyMonth : 0,
      months: frequencyMonths,
    },
  };
};

export function useMovementForm(
  options: UseMovementFormOptions = {},
): UseMovementFormResult {
  const [editingId, setEditingId] = useState<number | null>(null);
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
    setEditingId(null);
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
    if (!Number.isFinite(parsedAmount) || parsedAmount <= 0) {
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
    if (frequencyType === FrequencyType.Custom && customMonths.length < 1) {
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

    const request = buildRequest(
      name,
      amount,
      type,
      frequencyType,
      frequencyMonth,
      customMonths,
    );

    setSubmitting(true);
    try {
      if (editingId === null) {
        await MovementsService.create(request);
      } else {
        const updateRequest: UpdateMovementRequest = {
          ...request,
          id: editingId,
        };
        await MovementsService.update(editingId, updateRequest);
      }
      resetForm();
      if (options.onSaved) {
        await options.onSaved();
      }
    } catch (error) {
      setErrorMessage(
        editingId === null
          ? "No se pudo crear el movimiento. Por favor, inténtalo de nuevo."
          : "No se pudo actualizar el movimiento. Por favor, inténtalo de nuevo.",
      );
    } finally {
      setSubmitting(false);
    }
  }, [
    amount,
    customMonths,
    editingId,
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

  const startEdit = useCallback((movement: Movement) => {
    setEditingId(movement.id);
    setName(movement.name);
    setAmount(movement.amount.toString());
    setType(movement.type);
    setFrequencyType(movement.frequencyType);
    setFrequencyMonth(movement.frequencyMonth);
    setCustomMonths(toCustomMonths(movement.frequencyMonths));
    setErrorMessage(null);
    setValidationMessage(null);
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
    isEditing: editingId !== null,
    setName,
    setAmount,
    setType,
    setFrequencyType: handleFrequencyTypeChange,
    setFrequencyMonth,
    setCustomMonths: handleCustomMonthsChange,
    submit,
    startEdit,
    cancel: resetForm,
  };
}
