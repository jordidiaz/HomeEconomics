import { describe, it, expect, vi, beforeEach, afterEach } from "vitest";
import { renderHook, act, waitFor } from "@testing-library/react";
import { useMovementForm } from "../use-movement-form";
import { MovementsService } from "../../services/movements-service";
import { MovementType } from "../../types/movement-type";
import { FrequencyType } from "../../types/frequency-type";
import type { Movement } from "../../types/movement";

describe("useMovementForm", () => {
  const createMock = vi.spyOn(MovementsService, "create");
  const updateMock = vi.spyOn(MovementsService, "update");

  beforeEach(() => {
    createMock.mockReset();
    updateMock.mockReset();
  });

  afterEach(() => {
    createMock.mockReset();
    updateMock.mockReset();
  });

  it("shows validation when required fields are missing", async () => {
    const { result } = renderHook(() => useMovementForm());

    await act(async () => {
      await result.current.submit();
    });

    expect(result.current.validationMessage).toBe("Completa los campos requeridos.");
    expect(createMock).not.toHaveBeenCalled();
  });

  it("creates a movement and resets form", async () => {
    const onSaved = vi.fn();
    createMock.mockResolvedValueOnce(1);

    const { result } = renderHook(() => useMovementForm({ onSaved }));

    act(() => {
      result.current.setName(" Internet ");
      result.current.setAmount("35.5");
      result.current.setType(MovementType.Expense);
      result.current.setFrequencyType(FrequencyType.Monthly);
    });

    await act(async () => {
      await result.current.submit();
    });

    expect(createMock).toHaveBeenCalledWith({
      name: "Internet",
      amount: 35.5,
      type: MovementType.Expense,
      frequency: {
        type: FrequencyType.Monthly,
        month: 0,
        months: Array(12).fill(false),
      },
    });
    expect(onSaved).toHaveBeenCalled();
    expect(result.current.name).toBe("");
    expect(result.current.amount).toBe("");
    expect(result.current.type).toBe(MovementType.Undefined);
    expect(result.current.frequencyType).toBe(FrequencyType.Undefined);
  });

  it("updates an edited movement", async () => {
    updateMock.mockResolvedValueOnce();

    const movement: Movement = {
      id: 3,
      name: "Nomina",
      amount: 1200,
      type: MovementType.Income,
      frequencyType: FrequencyType.Yearly,
      frequencyMonth: 6,
      frequencyMonths: Array(12).fill(false),
    };

    const { result } = renderHook(() => useMovementForm());

    act(() => {
      result.current.startEdit(movement);
    });

    act(() => {
      result.current.setAmount("1300");
    });

    await act(async () => {
      await result.current.submit();
    });

    expect(updateMock).toHaveBeenCalledWith(3, {
      id: 3,
      name: "Nomina",
      amount: 1300,
      type: MovementType.Income,
      frequency: {
        type: FrequencyType.Yearly,
        month: 6,
        months: Array(12).fill(false),
      },
    });
  });

  it("requires custom months for custom frequency", async () => {
    const { result } = renderHook(() => useMovementForm());

    act(() => {
      result.current.setName("Extra");
      result.current.setAmount("10");
      result.current.setType(MovementType.Income);
      result.current.setFrequencyType(FrequencyType.Custom);
    });

    await act(async () => {
      await result.current.submit();
    });

    expect(result.current.validationMessage).toBe("Completa los campos requeridos.");
    expect(createMock).not.toHaveBeenCalled();
  });

  it("sets error message on submit failure", async () => {
    createMock.mockRejectedValueOnce(new Error("fail"));

    const { result } = renderHook(() => useMovementForm());

    act(() => {
      result.current.setName("Extra");
      result.current.setAmount("10");
      result.current.setType(MovementType.Income);
      result.current.setFrequencyType(FrequencyType.None);
    });

    await act(async () => {
      await result.current.submit();
    });

    await waitFor(() =>
      expect(result.current.errorMessage).toBe(
        "No se pudo crear el movimiento. Por favor, inténtalo de nuevo.",
      ),
    );
  });
});
