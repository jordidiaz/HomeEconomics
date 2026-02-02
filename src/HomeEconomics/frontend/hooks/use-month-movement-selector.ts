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
  creatingNextMonth: boolean;
  createNextMonthErrorMessage: string | null;
  creatingCurrentMonth: boolean;
  createCurrentMonthErrorMessage: string | null;
  nextMonthAvailable: boolean;
  currentMonthAvailable: boolean;
  createNextMonth: () => Promise<void>;
  createCurrentMonth: () => Promise<void>;
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
  const [creatingNextMonth, setCreatingNextMonth] = useState(false);
  const [createNextMonthErrorMessage, setCreateNextMonthErrorMessage] =
    useState<string | null>(null);
  const [creatingCurrentMonth, setCreatingCurrentMonth] = useState(false);
  const [createCurrentMonthErrorMessage, setCreateCurrentMonthErrorMessage] =
    useState<string | null>(null);
  const [nextMonthAvailable, setNextMonthAvailable] = useState(false);
  const [currentMonthAvailable, setCurrentMonthAvailable] = useState(true);
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
            setCurrentMonthAvailable(true);
          }
        }
      } catch (caughtError) {
        const status = (caughtError as { status?: number }).status;
        if (isMounted.current && updateAvailability && status === 404) {
          setMovementMonth(null);
          setCurrentMonthAvailable(false);
          setNextMonthAvailable(false);
          setError(null);
          return;
        }
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
    if (selectedMonth === "current" && !currentMonthAvailable) {
      return;
    }
    const target = selectedMonth === "current" ? currentMonth : nextMonth;
    await loadMovementMonth(target, selectedMonth === "current");
  }, [
    currentMonth,
    currentMonthAvailable,
    loadMovementMonth,
    nextMonth,
    nextMonthAvailable,
    selectedMonth,
  ]);

  const createNextMonth = useCallback(async () => {
    setCreatingNextMonth(true);
    setCreateNextMonthErrorMessage(null);
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
        setCreateNextMonthErrorMessage(
          "No se pudo crear el mes siguiente. Por favor, inténtalo de nuevo.",
        );
      }
    } finally {
      if (isMounted.current) {
        setCreatingNextMonth(false);
      }
    }
  }, [nextMonth.month, nextMonth.year]);

  const createCurrentMonth = useCallback(async () => {
    setCreatingCurrentMonth(true);
    setCreateCurrentMonthErrorMessage(null);
    try {
      const data = await MovementMonthsService.create(currentMonth.year, currentMonth.month);
      if (isMounted.current) {
        setCurrentMonthAvailable(true);
        setNextMonthAvailable(data.nextMovementMonthExists);
        setMovementMonth(data);
        setError(null);
        setSelectedMonth("current");
      }
    } catch (caughtError) {
      if (isMounted.current) {
        setCreateCurrentMonthErrorMessage(
          "No se pudo crear el mes actual. Por favor, inténtalo de nuevo.",
        );
      }
    } finally {
      if (isMounted.current) {
        setCreatingCurrentMonth(false);
      }
    }
  }, [currentMonth.month, currentMonth.year]);

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
    if (selectedMonth === "current" && !currentMonthAvailable) {
      return;
    }
    if (selectedMonth === "next" && skipNextLoad.current) {
      skipNextLoad.current = false;
      return;
    }
    const target = selectedMonth === "current" ? currentMonth : nextMonth;
    loadMovementMonth(target, selectedMonth === "current");
  }, [
    currentMonth,
    currentMonthAvailable,
    loadMovementMonth,
    nextMonth,
    nextMonthAvailable,
    selectedMonth,
  ]);

  return {
    currentMonth,
    nextMonth,
    selectedMonth,
    selectMonth: setSelectedMonth,
    movementMonth,
    loading,
    error,
    creatingNextMonth,
    createNextMonthErrorMessage,
    creatingCurrentMonth,
    createCurrentMonthErrorMessage,
    nextMonthAvailable,
    currentMonthAvailable,
    createNextMonth,
    createCurrentMonth,
    reloadSelectedMonth,
  };
}
