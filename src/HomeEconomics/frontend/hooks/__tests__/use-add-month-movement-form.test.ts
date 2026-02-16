import { describe, it, expect, vi, beforeEach, afterEach } from "vitest";
import { renderHook, act } from "@testing-library/react";
import { useAddMonthMovementForm } from "../use-add-month-movement-form";
import { MovementMonthsService } from "../../services/movement-months-service";
import { MovementType } from "../../types/movement-type";

describe("useAddMonthMovementForm", () => {
  const addMock = vi.spyOn(MovementMonthsService, "addMonthMovement");

  beforeEach(() => {
    addMock.mockReset();
  });

  afterEach(() => {
    addMock.mockReset();
  });

  it("validates required fields", async () => {
    const { result } = renderHook(() =>
      useAddMonthMovementForm({ movementMonthId: 10 }),
    );

    await act(async () => {
      await result.current.submit();
    });

    expect(result.current.validationMessage).toBe("Completa los campos requeridos.");
    expect(addMock).not.toHaveBeenCalled();
  });

  it("returns error when movement month is missing", async () => {
    const { result } = renderHook(() =>
      useAddMonthMovementForm({ movementMonthId: null }),
    );

    act(() => {
      result.current.setName("Seguro");
      result.current.setAmount("20");
      result.current.setType(MovementType.Expense);
    });

    await act(async () => {
      await result.current.submit();
    });

    expect(result.current.errorMessage).toBe(
      "No se pudo crear el movimiento. Por favor, inténtalo de nuevo.",
    );
  });

  it("submits and resets form", async () => {
    const onAdded = vi.fn();
    addMock.mockResolvedValueOnce({
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
    });

    const { result } = renderHook(() =>
      useAddMonthMovementForm({ movementMonthId: 10, onAdded }),
    );

    act(() => {
      result.current.setName(" Seguro ");
      result.current.setAmount("20");
      result.current.setType(MovementType.Expense);
    });

    await act(async () => {
      await result.current.submit();
    });

    expect(addMock).toHaveBeenCalledWith(10, {
      name: "Seguro",
      amount: 20,
      type: MovementType.Expense,
    });
    expect(onAdded).toHaveBeenCalled();
    expect(result.current.name).toBe("");
    expect(result.current.amount).toBe("");
    expect(result.current.type).toBe(MovementType.Undefined);
  });
});
