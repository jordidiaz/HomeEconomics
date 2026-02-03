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

type UseCurrentMonthMovementsResult = {
  monthMovements: MonthMovementListItem[];
  totalMonthMovements: number;
  showPaid: boolean;
  setShowPaid: (value: boolean) => void;
  loading: boolean;
  error: Error | null;
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
  const [showPaid, setShowPaid] = useState(false);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<Error | null>(null);
  const isMounted = useRef(true);

  const monthMovements = useMemo(
    () => allMonthMovements.filter((movement) => movement.paid === showPaid),
    [allMonthMovements, showPaid],
  );

  const loadMonthMovements = useCallback(async () => {
    setLoading(true);
    setError(null);
    try {
      const { year, month } = getCurrentYearMonth();
      const data = await MovementMonthsService.getByYearMonth(year, month);
      if (isMounted.current) {
        setAllMonthMovements(data.monthMovements.map(toMonthMovementListItem));
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
  }, []);

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
  };
}
