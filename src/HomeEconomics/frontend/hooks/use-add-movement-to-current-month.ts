import { useCallback, useState } from "react";
import { MovementMonthsService } from "../services/movement-months-service";
import type { Movement } from "../types/movement";

type MovementAddActionState = {
  loading: boolean;
  errorMessage: string | null;
};

type UseAddMovementToCurrentMonthOptions = {
  movementMonthId: number | null;
  onAdded?: () => Promise<void> | void;
};

type UseAddMovementToCurrentMonthResult = {
  actionStates: Record<number, MovementAddActionState>;
  addToCurrentMonth: (movement: Movement) => Promise<boolean>;
};

export function useAddMovementToCurrentMonth(
  options: UseAddMovementToCurrentMonthOptions,
): UseAddMovementToCurrentMonthResult {
  const [actionStates, setActionStates] = useState<Record<number, MovementAddActionState>>({});

  const updateActionState = useCallback(
    (movementId: number, changes: Partial<MovementAddActionState>) => {
      setActionStates((prev) => {
        const currentState = prev[movementId] ?? {
          loading: false,
          errorMessage: null,
        };
        return {
          ...prev,
          [movementId]: {
            ...currentState,
            ...changes,
          },
        };
      });
    },
    [],
  );

  const addToCurrentMonth = useCallback(
    async (movement: Movement) => {
      if (!options.movementMonthId) {
        return false;
      }
      updateActionState(movement.id, { loading: true, errorMessage: null });
      try {
        await MovementMonthsService.addMonthMovement(options.movementMonthId, {
          name: movement.name,
          amount: movement.amount,
          type: movement.type,
        });
        if (options.onAdded) {
          await options.onAdded();
        }
        return true;
      } catch (caughtError) {
        updateActionState(movement.id, {
          errorMessage:
            "No se pudo agregar el movimiento al mes actual. Por favor, inténtalo de nuevo.",
        });
        return false;
      } finally {
        updateActionState(movement.id, { loading: false });
      }
    },
    [options.movementMonthId, options.onAdded, updateActionState],
  );

  return { actionStates, addToCurrentMonth };
}
