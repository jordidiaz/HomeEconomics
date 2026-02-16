import { describe, it, expect, vi, beforeEach, afterEach } from "vitest";
import { renderHook, act, waitFor } from "@testing-library/react";
import { useCurrentMonthMovements } from "../use-current-month-movements";
import { MovementMonthsService } from "../../services/movement-months-service";
import type { MovementMonth } from "../../types/movement-month";
import { MovementType } from "../../types/movement-type";

const selectorMock = {
  movementMonth: null as MovementMonth | null,
  loading: false,
  error: null as Error | null,
  selectedMonth: "current" as const,
  selectMonth: vi.fn(),
  currentMonth: { year: 2024, month: 5 },
  nextMonth: { year: 2024, month: 6 },
  nextMonthAvailable: true,
  currentMonthAvailable: true,
  creatingNextMonth: false,
  createNextMonthErrorMessage: null as string | null,
  creatingCurrentMonth: false,
  createCurrentMonthErrorMessage: null as string | null,
  createNextMonth: vi.fn(),
  createCurrentMonth: vi.fn(),
  currentMovementMonthId: 10 as number | null,
  reloadSelectedMonth: vi.fn(),
};

vi.mock("../use-month-movement-selector", () => ({
  useMonthMovementSelector: () => selectorMock,
}));

const createMovementMonth = (): MovementMonth => ({
  id: 10,
  year: 2024,
  month: 5,
  nextMovementMonthExists: true,
  status: {
    pendingTotalExpenses: 0,
    pendingTotalIncomes: 0,
    accountAmount: 0,
    cashAmount: 0,
  },
  monthMovements: [
    { id: 1, name: "Seguro", amount: 20, type: MovementType.Expense, paid: false },
    { id: 2, name: "Nomina", amount: 1200, type: MovementType.Income, paid: true },
  ],
});

describe("useCurrentMonthMovements", () => {
  const payMock = vi.spyOn(MovementMonthsService, "payMonthMovement");
  const unpayMock = vi.spyOn(MovementMonthsService, "unpayMonthMovement");
  const updateAmountMock = vi.spyOn(MovementMonthsService, "updateMonthMovementAmount");
  const deleteMock = vi.spyOn(MovementMonthsService, "deleteMonthMovement");
  const moveMock = vi.spyOn(MovementMonthsService, "moveMonthMovementToNextMonth");

  beforeEach(() => {
    selectorMock.movementMonth = createMovementMonth();
    selectorMock.currentMovementMonthId = 10;
    selectorMock.reloadSelectedMonth = vi.fn();
    payMock.mockReset();
    unpayMock.mockReset();
    updateAmountMock.mockReset();
    deleteMock.mockReset();
    moveMock.mockReset();
  });

  afterEach(() => {
    payMock.mockReset();
    unpayMock.mockReset();
    updateAmountMock.mockReset();
    deleteMock.mockReset();
    moveMock.mockReset();
  });

  it("filters movements based on paid state", async () => {
    const { result } = renderHook(() => useCurrentMonthMovements());

    await waitFor(() => expect(result.current.monthMovements.length).toBe(1));
    expect(result.current.monthMovements[0].paid).toBe(false);

    act(() => {
      result.current.setShowPaid(true);
    });

    await waitFor(() => expect(result.current.monthMovements.length).toBe(1));
    expect(result.current.monthMovements[0].paid).toBe(true);
  });

  it("executes pay action and reloads", async () => {
    payMock.mockResolvedValueOnce();

    const { result } = renderHook(() => useCurrentMonthMovements());

    await act(async () => {
      await result.current.payMonthMovement(1);
    });

    expect(payMock).toHaveBeenCalledWith(10, 1);
    expect(selectorMock.reloadSelectedMonth).toHaveBeenCalled();
    expect(result.current.actionStates[1].loading).toBe(false);
  });

  it("validates amount input for updates", async () => {
    const { result } = renderHook(() => useCurrentMonthMovements());

    let outcome = true;
    await act(async () => {
      outcome = await result.current.updateMonthMovementAmount(1, "abc");
    });

    expect(outcome).toBe(false);
    expect(result.current.amountUpdateState.errorMessage).toBe(
      "Introduce un importe válido.",
    );
  });

  it("reports delete errors", async () => {
    deleteMock.mockRejectedValueOnce(new Error("fail"));

    const { result } = renderHook(() => useCurrentMonthMovements());

    await act(async () => {
      await result.current.deleteMonthMovement(1);
    });

    expect(result.current.deleteState.errorMessage).toBe(
      "No se pudo eliminar el movimiento. Por favor, inténtalo de nuevo.",
    );
  });

  it("reports move errors", async () => {
    moveMock.mockRejectedValueOnce(new Error("fail"));

    const { result } = renderHook(() => useCurrentMonthMovements());

    await act(async () => {
      await result.current.moveMonthMovementToNextMonth(1);
    });

    expect(result.current.moveState.errorMessage).toBe(
      "No se pudo mover el movimiento al mes siguiente. Por favor, inténtalo de nuevo.",
    );
  });
});
