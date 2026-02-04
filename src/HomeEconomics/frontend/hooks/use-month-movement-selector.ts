import { useCallback, useEffect, useMemo, useRef, useState } from "react";
import { MovementMonthsService } from "../services/movement-months-service";
import type { MovementMonth } from "../types/movement-month";

type MonthReference = {
  year: number;
  month: number;
};

type SelectedMonth = "current" | "next";

type UseMonthMovementSelectorResult = {
  currentMonth: MonthReference;
  nextMonth: MonthReference;
  selectedMonth: SelectedMonth;
  selectMonth: (value: SelectedMonth) => void;
  movementMonth: MovementMonth | null;
  loading: boolean;
  error: Error | null;
  creating: boolean;
  createErrorMessage: string | null;
  nextMonthAvailable: boolean;
  createNextMonth: () => Promise<void>;
  reloadSelectedMonth: () => Promise<void>;
};

const getCurrentYearMonth = (): MonthReference => {
  const now = new Date();
  return { year: now.getFullYear(), month: now.getMonth() + 1 };
};

const getNextYearMonth = (current: MonthReference): MonthReference => {
  if (current.month === 12) {
    return { year: current.year + 1, month: 1 };
  }
  return { year: current.year, month: current.month + 1 };
};

export function useMonthMovementSelector(): UseMonthMovementSelectorResult {
  const [selectedMonth, setSelectedMonth] = useState<SelectedMonth>("current");
  const [movementMonth, setMovementMonth] = useState<MovementMonth | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<Error | null>(null);
  const [creating, setCreating] = useState(false);
  const [createErrorMessage, setCreateErrorMessage] = useState<string | null>(null);
  const [nextMonthAvailable, setNextMonthAvailable] = useState(false);
  const isMounted = useRef(true);
  const skipNextLoad = useRef(false);

  const currentMonth = useMemo(() => getCurrentYearMonth(), []);
  const nextMonth = useMemo(() => getNextYearMonth(currentMonth), [currentMonth]);

  const loadMovementMonth = useCallback(
    async (target: MonthReference, updateAvailability: boolean) => {
      setLoading(true);
      setError(null);
      try {
        const data = await MovementMonthsService.getByYearMonth(target.year, target.month);
        if (isMounted.current) {
          setMovementMonth(data);
          if (updateAvailability) {
            setNextMonthAvailable(data.nextMovementMonthExists);
          }
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
    },
    [],
  );

  const reloadSelectedMonth = useCallback(async () => {
    if (selectedMonth === "next" && !nextMonthAvailable) {
      return;
    }
    const target = selectedMonth === "current" ? currentMonth : nextMonth;
    await loadMovementMonth(target, selectedMonth === "current");
  }, [currentMonth, loadMovementMonth, nextMonth, nextMonthAvailable, selectedMonth]);

  const createNextMonth = useCallback(async () => {
    setCreating(true);
    setCreateErrorMessage(null);
    try {
      const data = await MovementMonthsService.create(nextMonth.year, nextMonth.month);
      if (isMounted.current) {
        setNextMonthAvailable(true);
        setMovementMonth(data);
        setError(null);
        skipNextLoad.current = true;
        setSelectedMonth("next");
      }
    } catch (caughtError) {
      if (isMounted.current) {
        setCreateErrorMessage(
          "No se pudo crear el mes siguiente. Por favor, inténtalo de nuevo.",
        );
      }
    } finally {
      if (isMounted.current) {
        setCreating(false);
      }
    }
  }, [nextMonth.month, nextMonth.year]);

  useEffect(() => {
    isMounted.current = true;
    return () => {
      isMounted.current = false;
    };
  }, []);

  useEffect(() => {
    if (selectedMonth === "next" && !nextMonthAvailable) {
      return;
    }
    if (selectedMonth === "next" && skipNextLoad.current) {
      skipNextLoad.current = false;
      return;
    }
    const target = selectedMonth === "current" ? currentMonth : nextMonth;
    loadMovementMonth(target, selectedMonth === "current");
  }, [currentMonth, loadMovementMonth, nextMonth, nextMonthAvailable, selectedMonth]);

  return {
    currentMonth,
    nextMonth,
    selectedMonth,
    selectMonth: setSelectedMonth,
    movementMonth,
    loading,
    error,
    creating,
    createErrorMessage,
    nextMonthAvailable,
    createNextMonth,
    reloadSelectedMonth,
  };
}
