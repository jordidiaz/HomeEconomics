import { describe, it, expect, vi, beforeEach, afterEach } from "vitest";
import { renderHook, act } from "@testing-library/react";
import { useAddMovementToCurrentMonth } from "../use-add-movement-to-current-month";
import { MovementMonthsService } from "../../services/movement-months-service";
import type { Movement } from "../../types/movement";
import { MovementType } from "../../types/movement-type";
import { FrequencyType } from "../../types/frequency-type";

describe("useAddMovementToCurrentMonth", () => {
  const addMock = vi.spyOn(MovementMonthsService, "addMonthMovement");

  const movement: Movement = {
    id: 1,
    name: "Nomina",
    amount: 1200,
    type: MovementType.Income,
    frequencyType: FrequencyType.Monthly,
    frequencyMonth: 1,
    frequencyMonths: Array(12).fill(false),
  };

  beforeEach(() => {
    addMock.mockReset();
  });

  afterEach(() => {
    addMock.mockReset();
  });

  it("returns false when movementMonthId is missing", async () => {
    const { result } = renderHook(() =>
      useAddMovementToCurrentMonth({ movementMonthId: null }),
    );

    let outcome = true;
    await act(async () => {
      outcome = await result.current.addToCurrentMonth(movement);
    });

    expect(outcome).toBe(false);
    expect(addMock).not.toHaveBeenCalled();
  });

  it("updates action state on success", async () => {
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
      useAddMovementToCurrentMonth({ movementMonthId: 10 }),
    );

    await act(async () => {
      await result.current.addToCurrentMonth(movement);
    });

    expect(addMock).toHaveBeenCalledWith(10, {
      name: "Nomina",
      amount: 1200,
      type: MovementType.Income,
    });
    expect(result.current.actionStates[movement.id].loading).toBe(false);
    expect(result.current.actionStates[movement.id].errorMessage).toBeNull();
  });

  it("sets error message per item on failure", async () => {
    addMock.mockRejectedValueOnce(new Error("fail"));

    const { result } = renderHook(() =>
      useAddMovementToCurrentMonth({ movementMonthId: 10 }),
    );

    await act(async () => {
      await result.current.addToCurrentMonth(movement);
    });

    expect(result.current.actionStates[movement.id].errorMessage).toBe(
      "No se pudo agregar el movimiento al mes actual. Por favor, inténtalo de nuevo.",
    );
  });
});
