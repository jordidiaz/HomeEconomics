import { useEffect, useState } from "react";
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

  useEffect(() => {
    let isMounted = true;

    const loadMovements = async () => {
      try {
        const data = await MovementsService.getAll();
        if (isMounted) {
          setMovements(data.map(toMovementListItem));
        }
      } catch (caughtError) {
        if (isMounted) {
          setError(caughtError as Error);
        }
      } finally {
        if (isMounted) {
          setLoading(false);
        }
      }
    };

    loadMovements();

    return () => {
      isMounted = false;
    };
  }, []);

  return { movements, loading, error };
}
