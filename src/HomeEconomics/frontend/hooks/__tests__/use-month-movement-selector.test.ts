import { describe, it, expect, vi, beforeEach, afterEach } from "vitest";
import { renderHook, act } from "@testing-library/react";
import { useMonthMovementSelector } from "../use-month-movement-selector";
import { MovementMonthsService } from "../../services/movement-months-service";
import type { MovementMonth } from "../../types/movement-month";

const createMovementMonth = (overrides: Partial<MovementMonth> = {}): MovementMonth => ({
  id: 10,
  year: 2024,
  month: 5,
  nextMovementMonthExists: false,
  status: {
    pendingTotalExpenses: 0,
    pendingTotalIncomes: 0,
    accountAmount: 0,
    cashAmount: 0,
  },
  monthMovements: [],
  ...overrides,
});

describe("useMonthMovementSelector", () => {
  const getByYearMonthMock = vi.spyOn(MovementMonthsService, "getByYearMonth");
  const createMock = vi.spyOn(MovementMonthsService, "create");

  beforeEach(() => {
    vi.useFakeTimers();
    vi.setSystemTime(new Date("2024-05-10T12:00:00Z"));
    getByYearMonthMock.mockReset();
    createMock.mockReset();
  });

  afterEach(() => {
    vi.useRealTimers();
    getByYearMonthMock.mockReset();
    createMock.mockReset();
  });

  it("loads the current movement month on mount", async () => {
    const month = createMovementMonth({ year: 2024, month: 5, id: 7 });
    getByYearMonthMock.mockResolvedValueOnce(month);

    const { result } = renderHook(() => useMonthMovementSelector());

    await act(async () => {
      await Promise.resolve();
    });

    expect(getByYearMonthMock).toHaveBeenCalledWith(2024, 5);
    expect(result.current.movementMonth).toEqual(month);
    expect(result.current.currentMovementMonthId).toBe(7);
    expect(result.current.currentMonthAvailable).toBe(true);
  });

  it("handles missing current month with 404 status", async () => {
    const error = Object.assign(new Error("Not found"), { status: 404 });
    getByYearMonthMock.mockRejectedValueOnce(error);

    const { result } = renderHook(() => useMonthMovementSelector());

    await act(async () => {
      await Promise.resolve();
    });

    expect(result.current.movementMonth).toBeNull();
    expect(result.current.currentMonthAvailable).toBe(false);
    expect(result.current.nextMonthAvailable).toBe(false);
    expect(result.current.error).toBeNull();
  });

  it("creates the next month and selects it", async () => {
    const current = createMovementMonth({ year: 2024, month: 5, id: 8 });
    getByYearMonthMock.mockResolvedValueOnce(current);
    createMock.mockResolvedValueOnce(
      createMovementMonth({ year: 2024, month: 6, id: 9, nextMovementMonthExists: false }),
    );

    const { result } = renderHook(() => useMonthMovementSelector());

    await act(async () => {
      await Promise.resolve();
    });

    await act(async () => {
      await result.current.createNextMonth();
    });

    expect(createMock).toHaveBeenCalledWith(2024, 6);
    expect(result.current.selectedMonth).toBe("next");
    expect(result.current.nextMonthAvailable).toBe(true);
  });

  it("creates the current month when missing", async () => {
    const error = Object.assign(new Error("Not found"), { status: 404 });
    getByYearMonthMock.mockRejectedValueOnce(error);
    createMock.mockResolvedValueOnce(createMovementMonth({ year: 2024, month: 5, id: 11 }));

    const { result } = renderHook(() => useMonthMovementSelector());

    await act(async () => {
      await Promise.resolve();
    });

    await act(async () => {
      await result.current.createCurrentMonth();
    });

    expect(createMock).toHaveBeenCalledWith(2024, 5);
    expect(result.current.currentMonthAvailable).toBe(true);
    expect(result.current.selectedMonth).toBe("current");
  });
});
