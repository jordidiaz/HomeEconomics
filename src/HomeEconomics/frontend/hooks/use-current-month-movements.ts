import { useCallback, useEffect, useMemo, useRef, useState } from "react";
import { MovementMonthsService } from "../services/movement-months-service";
import { useMonthMovementSelector } from "./use-month-movement-selector";
import { MovementType } from "../types/movement-type";
import type { MonthMovement } from "../types/month-movement";
import type { MovementMonth } from "../types/movement-month";

type MonthMovementListItem = {
  id: number;
  name: string;
  amount: string;
  amountValue: number;
  type: MovementType;
  typeLabel: string;
  paid: boolean;
  paidLabel: string;
  starred: boolean;
};

type MonthMovementActionState = {
  loading: boolean;
  errorMessage: string | null;
};

type MonthMovementEditState = {
  loading: boolean;
  errorMessage: string | null;
  monthMovementId: number | null;
};

type MonthMovementDeleteState = {
  loading: boolean;
  errorMessage: string | null;
  monthMovementId: number | null;
};

type MonthMovementMoveState = {
  loading: boolean;
  errorMessage: string | null;
  monthMovementId: number | null;
};

type UseCurrentMonthMovementsResult = {
  monthMovements: MonthMovementListItem[];
  totalMonthMovements: number;
  showPaid: boolean;
  setShowPaid: (value: boolean) => void;
  searchTerm: string;
  setSearchTerm: (value: string) => void;
  loading: boolean;
  error: Error | null;
  status: MovementMonth["status"] | null;
  movementMonth: { id: number; year: number; month: number } | null;
  selectedMonth: "previous" | "current" | "next";
  selectMonth: (value: "previous" | "current" | "next") => void;
  currentMonth: { year: number; month: number };
  nextMonth: { year: number; month: number };
  previousMonth: { year: number; month: number };
  nextMonthAvailable: boolean;
  previousMonthAvailable: boolean;
  nextMovementMonthExists: boolean;
  currentMonthAvailable: boolean;
  currentMovementMonthId: number | null;
  creatingNextMonth: boolean;
  createNextMonthErrorMessage: string | null;
  createNextMonth: () => Promise<void>;
  creatingCurrentMonth: boolean;
  createCurrentMonthErrorMessage: string | null;
  createCurrentMonth: () => Promise<void>;
  reloadCurrentMonthMovements: () => Promise<void>;
  reloadSelectedMonthMovements: () => Promise<void>;
  movementMonthLoaded: boolean;
  actionStates: Record<number, MonthMovementActionState>;
  payMonthMovement: (monthMovementId: number) => Promise<void>;
  unpayMonthMovement: (monthMovementId: number) => Promise<void>;
  starMonthMovement: (monthMovementId: number) => Promise<void>;
  unstarMonthMovement: (monthMovementId: number) => Promise<void>;
  editState: MonthMovementEditState;
  setEditTarget: (monthMovementId: number | null) => void;
  updateMonthMovement: (
    monthMovementId: number,
    name: string,
    amountInput: string,
    type: MovementType,
  ) => Promise<boolean>;
  deleteState: MonthMovementDeleteState;
  setDeleteTarget: (monthMovementId: number | null) => void;
  deleteMonthMovement: (monthMovementId: number) => Promise<boolean>;
  moveState: MonthMovementMoveState;
  setMoveTarget: (monthMovementId: number | null) => void;
  moveMonthMovementToNextMonth: (monthMovementId: number) => Promise<boolean>;
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
  amountValue: monthMovement.amount,
  type: monthMovement.type,
  typeLabel: formatTypeLabel(monthMovement.type),
  paid: monthMovement.paid,
  paidLabel: formatPaidLabel(monthMovement.paid),
  starred: monthMovement.starred,
});

export function useCurrentMonthMovements(): UseCurrentMonthMovementsResult {
  const selector = useMonthMovementSelector();
  const [allMonthMovements, setAllMonthMovements] = useState<MonthMovementListItem[]>([]);
  const [showPaid, setShowPaid] = useState(false);
  const [searchTerm, setSearchTerm] = useState("");
  const [actionStates, setActionStates] = useState<Record<number, MonthMovementActionState>>(
    {},
  );
  const [editState, setEditState] = useState<MonthMovementEditState>({
    loading: false,
    errorMessage: null,
    monthMovementId: null,
  });
  const [deleteState, setDeleteState] = useState<MonthMovementDeleteState>({
    loading: false,
    errorMessage: null,
    monthMovementId: null,
  });
  const [moveState, setMoveState] = useState<MonthMovementMoveState>({
    loading: false,
    errorMessage: null,
    monthMovementId: null,
  });
  const isMounted = useRef(true);

  const monthMovements = useMemo(() => {
    const byPaid = allMonthMovements.filter((movement) => movement.paid === showPaid);
    if (!searchTerm) return byPaid;
    const lower = searchTerm.toLowerCase();
    return byPaid.filter(
      (movement) =>
        movement.name.toLowerCase().includes(lower) || movement.amount.includes(searchTerm),
    );
  }, [allMonthMovements, showPaid, searchTerm]);

  const movementMonthId = selector.movementMonth?.id ?? null;

  const reloadMonthMovements = useCallback(async () => {
    await selector.reloadSelectedMonth();
  }, [selector.reloadSelectedMonth]);

  const reloadCurrentMonthMovements = useCallback(async () => {
    if (selector.selectedMonth !== "current") {
      return;
    }
    await selector.reloadSelectedMonth();
  }, [selector.reloadSelectedMonth, selector.selectedMonth]);

  const reloadSelectedMonthMovements = useCallback(async () => {
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
    async (monthMovementId: number, action: "pay" | "unpay" | "star" | "unstar") => {
      if (!movementMonthId) {
        return;
      }
      updateActionState(monthMovementId, { loading: true, errorMessage: null });
      try {
        if (action === "pay") {
          await MovementMonthsService.payMonthMovement(movementMonthId, monthMovementId);
        } else if (action === "unpay") {
          await MovementMonthsService.unpayMonthMovement(movementMonthId, monthMovementId);
        } else if (action === "star") {
          await MovementMonthsService.starMonthMovement(movementMonthId, monthMovementId);
        } else {
          await MovementMonthsService.unstarMonthMovement(movementMonthId, monthMovementId);
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

  const setEditTarget = useCallback((monthMovementId: number | null) => {
    setEditState({
      loading: false,
      errorMessage: null,
      monthMovementId,
    });
  }, []);

  const updateMonthMovement = useCallback(
    async (monthMovementId: number, name: string, amountInput: string, type: MovementType) => {
      if (!movementMonthId) {
        return false;
      }
      const amount = Number(amountInput);
      setEditState({
        loading: true,
        errorMessage: null,
        monthMovementId,
      });

      if (Number.isNaN(amount)) {
        setEditState({
          loading: false,
          errorMessage: "Introduce un importe válido.",
          monthMovementId,
        });
        return false;
      }

      try {
        await MovementMonthsService.updateMonthMovement(
          movementMonthId,
          monthMovementId,
          { name, amount, type },
        );
        await reloadMonthMovements();
        return true;
      } catch (caughtError) {
        if (isMounted.current) {
          setEditState({
            loading: false,
            errorMessage:
              "No se pudo actualizar el movimiento. Por favor, inténtalo de nuevo.",
            monthMovementId,
          });
        }
        return false;
      } finally {
        if (isMounted.current) {
          setEditState((prev) => ({
            ...prev,
            loading: false,
          }));
        }
      }
    },
    [movementMonthId, reloadMonthMovements],
  );

  const setDeleteTarget = useCallback((monthMovementId: number | null) => {
    setDeleteState({
      loading: false,
      errorMessage: null,
      monthMovementId,
    });
  }, []);

  const deleteMonthMovement = useCallback(
    async (monthMovementId: number) => {
      if (!movementMonthId) {
        return false;
      }
      setDeleteState({
        loading: true,
        errorMessage: null,
        monthMovementId,
      });

      try {
        await MovementMonthsService.deleteMonthMovement(movementMonthId, monthMovementId);
        await reloadMonthMovements();
        return true;
      } catch (caughtError) {
        if (isMounted.current) {
          setDeleteState({
            loading: false,
            errorMessage:
              "No se pudo eliminar el movimiento. Por favor, inténtalo de nuevo.",
            monthMovementId,
          });
        }
        return false;
      } finally {
        if (isMounted.current) {
          setDeleteState((prev) => ({
            ...prev,
            loading: false,
          }));
        }
      }
    },
    [movementMonthId, reloadMonthMovements],
  );

  const setMoveTarget = useCallback((monthMovementId: number | null) => {
    setMoveState({
      loading: false,
      errorMessage: null,
      monthMovementId,
    });
  }, []);

  const moveMonthMovementToNextMonth = useCallback(
    async (monthMovementId: number) => {
      if (!movementMonthId) {
        return false;
      }
      setMoveState({
        loading: true,
        errorMessage: null,
        monthMovementId,
      });

      try {
        await MovementMonthsService.moveMonthMovementToNextMonth(
          movementMonthId,
          monthMovementId,
        );
        await reloadMonthMovements();
        return true;
      } catch (caughtError) {
        if (isMounted.current) {
          setMoveState({
            loading: false,
            errorMessage:
              "No se pudo mover el movimiento al mes siguiente. Por favor, inténtalo de nuevo.",
            monthMovementId,
          });
        }
        return false;
      } finally {
        if (isMounted.current) {
          setMoveState((prev) => ({
            ...prev,
            loading: false,
          }));
        }
      }
    },
    [movementMonthId, reloadMonthMovements],
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

  const starMonthMovement = useCallback(
    async (monthMovementId: number) => {
      await handleMonthMovementAction(monthMovementId, "star");
    },
    [handleMonthMovementAction],
  );

  const unstarMonthMovement = useCallback(
    async (monthMovementId: number) => {
      await handleMonthMovementAction(monthMovementId, "unstar");
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
    setEditState({ loading: false, errorMessage: null, monthMovementId: null });
    setDeleteState({ loading: false, errorMessage: null, monthMovementId: null });
    setMoveState({ loading: false, errorMessage: null, monthMovementId: null });
  }, [selector.movementMonth]);

  return {
    monthMovements,
    totalMonthMovements: allMonthMovements.length,
    showPaid,
    setShowPaid,
    searchTerm,
    setSearchTerm,
    loading: selector.loading || selector.creatingNextMonth || selector.creatingCurrentMonth,
    error: selector.error,
    status: selector.movementMonth?.status ?? null,
    movementMonth: selector.movementMonth
      ? {
          id: selector.movementMonth.id,
          year: selector.movementMonth.year,
          month: selector.movementMonth.month,
        }
      : null,
    selectedMonth: selector.selectedMonth,
    selectMonth: selector.selectMonth,
    currentMonth: selector.currentMonth,
    nextMonth: selector.nextMonth,
    previousMonth: selector.previousMonth,
    nextMonthAvailable: selector.nextMonthAvailable,
    previousMonthAvailable: selector.previousMonthAvailable,
    nextMovementMonthExists: selector.movementMonth?.nextMovementMonthExists ?? false,
    currentMonthAvailable: selector.currentMonthAvailable,
    currentMovementMonthId: selector.currentMovementMonthId,
    creatingNextMonth: selector.creatingNextMonth,
    createNextMonthErrorMessage: selector.createNextMonthErrorMessage,
    createNextMonth: selector.createNextMonth,
    creatingCurrentMonth: selector.creatingCurrentMonth,
    createCurrentMonthErrorMessage: selector.createCurrentMonthErrorMessage,
    createCurrentMonth: selector.createCurrentMonth,
    reloadCurrentMonthMovements,
    reloadSelectedMonthMovements,
    movementMonthLoaded: selector.movementMonth !== null,
    actionStates,
    payMonthMovement,
    unpayMonthMovement,
    starMonthMovement,
    unstarMonthMovement,
    editState,
    setEditTarget,
    updateMonthMovement,
    deleteState,
    setDeleteTarget,
    deleteMonthMovement,
    moveState,
    setMoveTarget,
    moveMonthMovementToNextMonth,
  };
}
