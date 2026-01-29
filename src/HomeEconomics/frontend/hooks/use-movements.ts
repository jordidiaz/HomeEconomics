import { useCallback, useEffect, useRef, useState } from "react";
import { MovementsService } from "../services/movements-service";
import { FrequencyType } from "../types/frequency-type";
import { MovementType } from "../types/movement-type";
import type { Movement } from "../types/movement";

type MovementListItem = {
  id: number;
  name: string;
  amount: string;
  type: MovementType;
  typeLabel: string;
  frequencyLabel: string;
};

type UseMovementsResult = {
  movements: MovementListItem[];
  loading: boolean;
  error: Error | null;
  reload: () => Promise<void>;
};

const monthLabels = [
  "Ene",
  "Feb",
  "Mar",
  "Abr",
  "May",
  "Jun",
  "Jul",
  "Ago",
  "Sep",
  "Oct",
  "Nov",
  "Dic",
];

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

const formatFrequencyLabel = (movement: Movement): string => {
  switch (movement.frequencyType) {
    case FrequencyType.None:
      return "Único";
    case FrequencyType.Monthly:
      return "Mensual";
    case FrequencyType.Yearly: {
      const monthIndex = movement.frequencyMonth - 1;
      return monthLabels[monthIndex] ?? "Anual";
    }
    case FrequencyType.Custom: {
      const months = movement.frequencyMonths
        .map((isSelected, index) => (isSelected ? monthLabels[index] : null))
        .filter((month): month is string => Boolean(month));
      return months.length > 0 ? months.join(", ") : "Personalizado";
    }
    default:
      return "Desconocido";
  }
};

const toMovementListItem = (movement: Movement): MovementListItem => ({
  id: movement.id,
  name: movement.name,
  amount: formatAmount(movement.amount),
  type: movement.type,
  typeLabel: formatTypeLabel(movement.type),
  frequencyLabel: formatFrequencyLabel(movement),
});

export function useMovements(): UseMovementsResult {
  const [movements, setMovements] = useState<MovementListItem[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<Error | null>(null);
  const isMounted = useRef(true);

  const loadMovements = useCallback(async () => {
    setLoading(true);
    setError(null);
    try {
      const data = await MovementsService.getAll();
      if (isMounted.current) {
        setMovements(data.map(toMovementListItem));
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
    isMounted.current = true; // Reset on mount
    loadMovements();

    return () => {
      isMounted.current = false;
    };
  }, [loadMovements]);

  return { movements, loading, error, reload: loadMovements };
}
