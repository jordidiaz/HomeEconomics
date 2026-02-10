import { useCallback, useMemo, useState } from "react";
import { MovementMonthsService } from "../services/movement-months-service";
import { MovementType } from "../types/movement-type";

type UseAddMonthMovementFormOptions = {
  movementMonthId: number | null;
  onAdded?: () => Promise<void> | void;
};

type UseAddMonthMovementFormResult = {
  name: string;
  amount: string;
  type: MovementType;
  submitting: boolean;
  errorMessage: string | null;
  validationMessage: string | null;
  setName: (value: string) => void;
  setAmount: (value: string) => void;
  setType: (value: MovementType) => void;
  submit: () => Promise<void>;
  cancel: () => void;
};

export function useAddMonthMovementForm(
  options: UseAddMonthMovementFormOptions,
): UseAddMonthMovementFormResult {
  const [name, setName] = useState("");
  const [amount, setAmount] = useState("");
  const [type, setType] = useState<MovementType>(MovementType.Undefined);
  const [submitting, setSubmitting] = useState(false);
  const [errorMessage, setErrorMessage] = useState<string | null>(null);
  const [validationMessage, setValidationMessage] = useState<string | null>(null);

  const resetForm = useCallback(() => {
    setName("");
    setAmount("");
    setType(MovementType.Undefined);
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
    return true;
  }, [amount, name, type]);

  const submit = useCallback(async () => {
    setValidationMessage(null);
    setErrorMessage(null);

    if (!isValid) {
      setValidationMessage("Completa los campos requeridos.");
      return;
    }

    if (!options.movementMonthId) {
      setErrorMessage(
        "No se pudo crear el movimiento. Por favor, inténtalo de nuevo.",
      );
      return;
    }

    const parsedAmount = Number(amount);

    setSubmitting(true);
    try {
      await MovementMonthsService.addMonthMovement(options.movementMonthId, {
        name: name.trim(),
        amount: parsedAmount,
        type,
      });
      resetForm();
      if (options.onAdded) {
        await options.onAdded();
      }
    } catch (error) {
      setErrorMessage(
        "No se pudo crear el movimiento. Por favor, inténtalo de nuevo.",
      );
    } finally {
      setSubmitting(false);
    }
  }, [amount, isValid, name, options, resetForm, type]);

  return {
    name,
    amount,
    type,
    submitting,
    errorMessage,
    validationMessage,
    setName,
    setAmount,
    setType,
    submit,
    cancel: resetForm,
  };
}
