import { describe, it, expect, vi, beforeEach, afterEach } from "vitest";
import { renderHook, act } from "@testing-library/react";
import { useDeleteMovement } from "../use-delete-movement";
import { MovementsService } from "../../services/movements-service";

describe("useDeleteMovement", () => {
  const deleteMock = vi.spyOn(MovementsService, "delete");

  beforeEach(() => {
    deleteMock.mockReset();
  });

  afterEach(() => {
    deleteMock.mockReset();
  });

  it("deletes and calls onDeleted", async () => {
    const onDeleted = vi.fn();
    deleteMock.mockResolvedValueOnce();

    const { result } = renderHook(() => useDeleteMovement({ onDeleted }));

    let outcome = false;
    await act(async () => {
      outcome = await result.current.deleteMovement(3);
    });

    expect(outcome).toBe(true);
    expect(deleteMock).toHaveBeenCalledWith(3);
    expect(onDeleted).toHaveBeenCalled();
    expect(result.current.deleting).toBe(false);
    expect(result.current.deletingId).toBeNull();
  });

  it("returns false and sets error on failure", async () => {
    deleteMock.mockRejectedValueOnce(new Error("fail"));

    const { result } = renderHook(() => useDeleteMovement());

    let outcome = true;
    await act(async () => {
      outcome = await result.current.deleteMovement(2);
    });

    expect(outcome).toBe(false);
    expect(result.current.errorMessage).toBe(
      "No se pudo eliminar el movimiento. Por favor, inténtalo de nuevo.",
    );

    act(() => {
      result.current.clearError();
    });

    expect(result.current.errorMessage).toBeNull();
  });
});
