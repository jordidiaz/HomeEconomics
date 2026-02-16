import { describe, it, expect, vi, beforeEach, afterEach } from "vitest";
import { renderHook, act } from "@testing-library/react";
import { useMovementMonthStatusForm } from "../use-movement-month-status-form";
import { MovementMonthsService } from "../../services/movement-months-service";

describe("useMovementMonthStatusForm", () => {
  const addStatusMock = vi.spyOn(MovementMonthsService, "addStatus");

  beforeEach(() => {
    addStatusMock.mockReset();
  });

  afterEach(() => {
    addStatusMock.mockReset();
  });

  it("calculates balance from inputs and pending amounts", () => {
    const { result } = renderHook(() =>
      useMovementMonthStatusForm({
        movementMonth: { id: 1, year: 2024, month: 5 },
        status: {
          pendingTotalExpenses: 50,
          pendingTotalIncomes: 10,
          accountAmount: 100,
          cashAmount: 20,
        },
      }),
    );

    expect(result.current.balance).toBe(80);
  });

  it("validates numeric input", async () => {
    const movementMonth = { id: 1, year: 2024, month: 5 };
    const status = {
      pendingTotalExpenses: 0,
      pendingTotalIncomes: 0,
      accountAmount: 0,
      cashAmount: 0,
    };
    const { result } = renderHook(() =>
      useMovementMonthStatusForm({
        movementMonth,
        status,
      }),
    );

    await act(async () => {
      await Promise.resolve();
    });

    act(() => {
      result.current.setAccountAmount("-1");
    });

    await act(async () => {
      await result.current.submitOnBlur();
    });

    expect(result.current.errorMessage).toBe("Introduce valores numericos validos.");
    expect(addStatusMock).not.toHaveBeenCalled();
  });

  it("dedupes submits when values are unchanged", async () => {
    addStatusMock.mockResolvedValueOnce();

    const movementMonth = { id: 1, year: 2024, month: 5 };
    const status = {
      pendingTotalExpenses: 0,
      pendingTotalIncomes: 0,
      accountAmount: 100,
      cashAmount: 20,
    };

    const { result } = renderHook(() =>
      useMovementMonthStatusForm({
        movementMonth,
        status,
      }),
    );

    await act(async () => {
      await Promise.resolve();
    });

    await act(async () => {
      await result.current.submitOnBlur();
    });

    expect(addStatusMock).not.toHaveBeenCalled();

    act(() => {
      result.current.setCashAmount("25.00");
    });

    await act(async () => {
      await result.current.submitOnBlur();
    });

    expect(addStatusMock).toHaveBeenCalledWith(1, {
      year: 2024,
      month: 5,
      accountAmount: 100,
      cashAmount: 25,
    });
  });

  it("sets error message on submit failure", async () => {
    addStatusMock.mockRejectedValueOnce(new Error("fail"));

    const movementMonth = { id: 1, year: 2024, month: 5 };
    const status = {
      pendingTotalExpenses: 0,
      pendingTotalIncomes: 0,
      accountAmount: 10,
      cashAmount: 5,
    };

    const { result } = renderHook(() =>
      useMovementMonthStatusForm({
        movementMonth,
        status,
      }),
    );

    await act(async () => {
      await Promise.resolve();
    });

    act(() => {
      result.current.setAccountAmount("12.00");
    });

    await act(async () => {
      await result.current.submitOnBlur();
    });

    expect(result.current.errorMessage).toBe(
      "No se pudo actualizar el estado del mes. Por favor, inténtalo de nuevo.",
    );
  });
});
