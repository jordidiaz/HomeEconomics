import { describe, it, expect, vi, beforeEach, afterEach } from "vitest";
import { renderHook, waitFor, act } from "@testing-library/react";
import { useMovements } from "../use-movements";
import { MovementsService } from "../../services/movements-service";
import { FrequencyType } from "../../types/frequency-type";
import { MovementType } from "../../types/movement-type";
import type { Movement } from "../../types/movement";

describe("useMovements", () => {
  const getAllMock = vi.spyOn(MovementsService, "getAll");

  beforeEach(() => {
    getAllMock.mockReset();
  });

  afterEach(() => {
    getAllMock.mockReset();
  });

  it("loads movements and formats list labels", async () => {
    const movements: Movement[] = [
      {
        id: 1,
        name: "Nomina",
        amount: 1250.5,
        type: MovementType.Income,
        frequencyType: FrequencyType.Monthly,
        frequencyMonth: 1,
        frequencyMonths: Array(12).fill(false),
      },
      {
        id: 2,
        name: "Seguro",
        amount: 20,
        type: MovementType.Expense,
        frequencyType: FrequencyType.Yearly,
        frequencyMonth: 5,
        frequencyMonths: Array(12).fill(false),
      },
      {
        id: 3,
        name: "Extra",
        amount: 300,
        type: MovementType.Undefined,
        frequencyType: FrequencyType.Custom,
        frequencyMonth: 1,
        frequencyMonths: [
          true,
          false,
          true,
          false,
          false,
          false,
          false,
          false,
          false,
          false,
          false,
          false,
        ],
      },
      {
        id: 4,
        name: "Evento",
        amount: 10,
        type: MovementType.Expense,
        frequencyType: FrequencyType.None,
        frequencyMonth: 1,
        frequencyMonths: Array(12).fill(false),
      },
    ];

    getAllMock.mockResolvedValueOnce(movements);

    const { result } = renderHook(() => useMovements());

    await waitFor(() => expect(result.current.loading).toBe(false));

    const amountFormatter = new Intl.NumberFormat("es-ES", {
      minimumFractionDigits: 2,
      maximumFractionDigits: 2,
    });

    expect(result.current.error).toBeNull();
    expect(result.current.movements).toHaveLength(4);
    expect(result.current.movements[0]).toEqual({
      id: 1,
      name: "Nomina",
      amount: amountFormatter.format(1250.5),
      type: MovementType.Income,
      typeLabel: "Ingreso",
      frequencyLabel: "Mensual",
    });
    expect(result.current.movements[1].frequencyLabel).toBe("May");
    expect(result.current.movements[2].typeLabel).toBe("Desconocido");
    expect(result.current.movements[2].frequencyLabel).toBe("Ene, Mar");
    expect(result.current.movements[3].frequencyLabel).toBe("Único");
    expect(result.current.movementMap[2]).toEqual(movements[1]);
  });

  it("exposes error when the service fails", async () => {
    const failure = new Error("Boom");
    getAllMock.mockRejectedValueOnce(failure);

    const { result } = renderHook(() => useMovements());

    await waitFor(() => expect(result.current.loading).toBe(false));

    expect(result.current.error).toBe(failure);
    expect(result.current.movements).toEqual([]);
  });

  it("reloads data on demand", async () => {
    const movements: Movement[] = [
      {
        id: 1,
        name: "Nomina",
        amount: 1000,
        type: MovementType.Income,
        frequencyType: FrequencyType.Monthly,
        frequencyMonth: 1,
        frequencyMonths: Array(12).fill(false),
      },
    ];

    getAllMock.mockResolvedValue(movements);

    const { result } = renderHook(() => useMovements());

    await waitFor(() => expect(result.current.loading).toBe(false));

    await act(async () => {
      await result.current.reload();
    });

    expect(getAllMock).toHaveBeenCalledTimes(2);
  });
});
