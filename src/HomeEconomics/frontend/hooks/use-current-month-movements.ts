import { useCallback, useEffect, useMemo, useRef, useState } from "react";
import { MovementMonthsService } from "../services/movement-months-service";
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

const getCurrentYearMonth = (): { year: number; month: number } => {
  const now = new Date();
  return { year: now.getFullYear(), month: now.getMonth() + 1 };
};

export function useCurrentMonthMovements(): UseCurrentMonthMovementsResult {
  const [allMonthMovements, setAllMonthMovements] = useState<MonthMovementListItem[]>([]);
  const [movementMonthId, setMovementMonthId] = useState<number | null>(null);
  const [showPaid, setShowPaid] = useState(false);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<Error | null>(null);
  const [actionStates, setActionStates] = useState<Record<number, MonthMovementActionState>>(
    {},
  );
  const isMounted = useRef(true);

  const monthMovements = useMemo(
    () => allMonthMovements.filter((movement) => movement.paid === showPaid),
    [allMonthMovements, showPaid],
  );

  const fetchMonthMovements = useCallback(async () => {
    const { year, month } = getCurrentYearMonth();
    return MovementMonthsService.getByYearMonth(year, month);
  }, []);

  const loadMonthMovements = useCallback(async () => {
    setLoading(true);
    setError(null);
    try {
      const data = await fetchMonthMovements();
      if (isMounted.current) {
        setAllMonthMovements(data.monthMovements.map(toMonthMovementListItem));
        setMovementMonthId(data.id);
      }
    } catch (caughtError) {
      if (isMounted.current) {
        setError(caughtError as Error);
      }
    } finally {
      if (isMounted.current) {
        setLoading(false);
      }
    }
  }, [fetchMonthMovements]);

  const reloadMonthMovements = useCallback(async () => {
    const data = await fetchMonthMovements();
    if (isMounted.current) {
      setAllMonthMovements(data.monthMovements.map(toMonthMovementListItem));
      setMovementMonthId(data.id);
    }
  }, [fetchMonthMovements]);

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
    loadMonthMovements();

    return () => {
      isMounted.current = false;
    };
  }, [loadMonthMovements]);

  return {
    monthMovements,
    totalMonthMovements: allMonthMovements.length,
    showPaid,
    setShowPaid,
    loading,
    error,
    actionStates,
    payMonthMovement,
    unpayMonthMovement,
  };
}
