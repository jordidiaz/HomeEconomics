import React from "react";
import { describe, it, expect, vi } from "vitest";
import { fireEvent, render, screen } from "@testing-library/react";
import { ConfirmMoveMonthMovementDialog } from "../confirm-move-month-movement-dialog";

describe("ConfirmMoveMonthMovementDialog", () => {
  it("renders message and fires actions", () => {
    const onCancel = vi.fn();
    const onConfirm = vi.fn();

    render(
      <ConfirmMoveMonthMovementDialog
        open={true}
        moving={false}
        errorMessage={null}
        movementName="Seguro"
        onCancel={onCancel}
        onConfirm={onConfirm}
      />,
    );

    expect(screen.getByText("Confirmar acción")).toBeInTheDocument();
    expect(
      screen.getByText(
        "¿Quieres mover el movimiento \"Seguro\" al mes siguiente? Esta acción es irreversible y se moverá permanentemente.",
      ),
    ).toBeInTheDocument();

    fireEvent.click(screen.getByRole("button", { name: "Cancelar" }));
    fireEvent.click(screen.getByRole("button", { name: "Aceptar" }));

    expect(onCancel).toHaveBeenCalled();
    expect(onConfirm).toHaveBeenCalled();
  });

  it("disables buttons and shows error when moving", () => {
    render(
      <ConfirmMoveMonthMovementDialog
        open={true}
        moving={true}
        errorMessage="Error"
        movementName="Seguro"
        onCancel={vi.fn()}
        onConfirm={vi.fn()}
      />,
    );

    expect(screen.getByRole("button", { name: "Cancelar" })).toBeDisabled();
    expect(screen.getByRole("button", { name: "Aceptar" })).toBeDisabled();
    expect(screen.getByText("Error")).toBeInTheDocument();
  });
});
