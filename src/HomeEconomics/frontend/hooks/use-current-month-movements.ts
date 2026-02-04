import { useCallback, useEffect, useMemo, useRef, useState } from "react";
import { MovementMonthsService } from "../services/movement-months-service";
import { useMonthMovementSelector } from "./use-month-movement-selector";
import { MovementType } from "../types/movement-type";
import type { MonthMovement } from "../types/month-movement";

type MonthMovementListItem = {
  id: number;
  name: string;
  amount: string;
  type: MovementType;
  typeLabel: string;
  paid: boolean;
  paidLabel: string;
};

type MonthMovementActionState = {
  loading: boolean;
  errorMessage: string | null;
};

type UseCurrentMonthMovementsResult = {
  monthMovements: MonthMovementListItem[];
  totalMonthMovements: number;
  showPaid: boolean;
  setShowPaid: (value: boolean) => void;
  loading: boolean;
  error: Error | null;
  selectedMonth: "current" | "next";
  selectMonth: (value: "current" | "next") => void;
  currentMonth: { year: number; month: number };
  nextMonth: { year: number; month: number };
  nextMonthAvailable: boolean;
  creatingNextMonth: boolean;
  createNextMonthErrorMessage: string | null;
  createNextMonth: () => Promise<void>;
  movementMonthLoaded: boolean;
  actionStates: Record<number, MonthMovementActionState>;
  payMonthMovement: (monthMovementId: number) => Promise<void>;
  unpayMonthMovement: (monthMovementId: number) => Promise<void>;
};

const formatAmount = (amount: number): string =>
  new Intl.NumberFormat("es-ES", {
    minimumFractionDigits: 2,
    maximumFractionDigits: 2,
  }).format(amount);

const formatTypeLabel = (type: MovementType): string => {
  switch (type) {
    case MovementType.Income:
      return "Ingreso";
    case MovementType.Expense:
      return "Gasto";
    default:
      return "Desconocido";
  }
};

const formatPaidLabel = (paid: boolean): string =>
  paid ? "Pagado" : "Pendiente";

const toMonthMovementListItem = (monthMovement: MonthMovement): MonthMovementListItem => ({
  id: monthMovement.id,
  name: monthMovement.name,
  amount: formatAmount(monthMovement.amount),
  type: monthMovement.type,
  typeLabel: formatTypeLabel(monthMovement.type),
  paid: monthMovement.paid,
  paidLabel: formatPaidLabel(monthMovement.paid),
});

export function useCurrentMonthMovements(): UseCurrentMonthMovementsResult {
  const selector = useMonthMovementSelector();
  const [allMonthMovements, setAllMonthMovements] = useState<MonthMovementListItem[]>([]);
  const [showPaid, setShowPaid] = useState(false);
  const [actionStates, setActionStates] = useState<Record<number, MonthMovementActionState>>(
    {},
  );
  const isMounted = useRef(true);

  const monthMovements = useMemo(
    () => allMonthMovements.filter((movement) => movement.paid === showPaid),
    [allMonthMovements, showPaid],
  );

  const movementMonthId = selector.movementMonth?.id ?? null;

  const reloadMonthMovements = useCallback(async () => {
    await selector.reloadSelectedMonth();
  }, [selector.reloadSelectedMonth]);

  const updateActionState = useCallback(
    (monthMovementId: number, changes: Partial<MonthMovementActionState>) => {
      setActionStates((prev) => {
        const currentState = prev[monthMovementId] ?? {
          loading: false,
          errorMessage: null,
        };
        return {
          ...prev,
          [monthMovementId]: {
            ...currentState,
            ...changes,
          },
        };
      });
    },
    [],
  );

  const handleMonthMovementAction = useCallback(
    async (monthMovementId: number, action: "pay" | "unpay") => {
      if (!movementMonthId) {
        return;
      }
      updateActionState(monthMovementId, { loading: true, errorMessage: null });
      try {
        if (action === "pay") {
          await MovementMonthsService.payMonthMovement(movementMonthId, monthMovementId);
        } else {
          await MovementMonthsService.unpayMonthMovement(movementMonthId, monthMovementId);
        }
        await reloadMonthMovements();
      } catch (caughtError) {
        if (isMounted.current) {
          updateActionState(monthMovementId, {
            errorMessage:
              "No se pudo actualizar el estado del movimiento. Por favor, inténtalo de nuevo.",
          });
        }
      } finally {
        if (isMounted.current) {
          updateActionState(monthMovementId, { loading: false });
        }
      }
    },
    [movementMonthId, reloadMonthMovements, updateActionState],
  );

  const payMonthMovement = useCallback(
    async (monthMovementId: number) => {
      await handleMonthMovementAction(monthMovementId, "pay");
    },
    [handleMonthMovementAction],
  );

  const unpayMonthMovement = useCallback(
    async (monthMovementId: number) => {
      await handleMonthMovementAction(monthMovementId, "unpay");
    },
    [handleMonthMovementAction],
  );

  useEffect(() => {
    isMounted.current = true;
    return () => {
      isMounted.current = false;
    };
  }, []);

  useEffect(() => {
    if (selector.movementMonth) {
      setAllMonthMovements(selector.movementMonth.monthMovements.map(toMonthMovementListItem));
    } else {
      setAllMonthMovements([]);
    }
    setActionStates({});
  }, [selector.movementMonth]);

  return {
    monthMovements,
    totalMonthMovements: allMonthMovements.length,
    showPaid,
    setShowPaid,
    loading: selector.loading || selector.creating,
    error: selector.error,
    selectedMonth: selector.selectedMonth,
    selectMonth: selector.selectMonth,
    currentMonth: selector.currentMonth,
    nextMonth: selector.nextMonth,
    nextMonthAvailable: selector.nextMonthAvailable,
    creatingNextMonth: selector.creating,
    createNextMonthErrorMessage: selector.createErrorMessage,
    createNextMonth: selector.createNextMonth,
    movementMonthLoaded: selector.movementMonth !== null,
    actionStates,
    payMonthMovement,
    unpayMonthMovement,
  };
}
