import React from "react";
import { describe, it, expect, vi } from "vitest";
import { fireEvent, render, screen } from "@testing-library/react";
import { ConfirmDeleteMovementDialog } from "../confirm-delete-movement-dialog";

describe("ConfirmDeleteMovementDialog", () => {
  it("renders message and fires actions", () => {
    const onCancel = vi.fn();
    const onConfirm = vi.fn();

    render(
      <ConfirmDeleteMovementDialog
        open={true}
        deleting={false}
        errorMessage={null}
        movementName="Nomina"
        onCancel={onCancel}
        onConfirm={onConfirm}
      />,
    );

    expect(screen.getByText("Confirmar borrado")).toBeInTheDocument();
    expect(
      screen.getByText(
        "¿Quieres eliminar el movimiento \"Nomina\"? Esta acción es irreversible y se eliminará permanentemente.",
      ),
    ).toBeInTheDocument();

    fireEvent.click(screen.getByRole("button", { name: "Cancelar" }));
    fireEvent.click(screen.getByRole("button", { name: "Eliminar" }));

    expect(onCancel).toHaveBeenCalled();
    expect(onConfirm).toHaveBeenCalled();
  });

  it("disables buttons and shows error when deleting", () => {
    render(
      <ConfirmDeleteMovementDialog
        open={true}
        deleting={true}
        errorMessage="Error"
        movementName="Nomina"
        onCancel={vi.fn()}
        onConfirm={vi.fn()}
      />,
    );

    expect(screen.getByRole("button", { name: "Cancelar" })).toBeDisabled();
    expect(screen.getByRole("button", { name: "Eliminar" })).toBeDisabled();
    expect(screen.getByText("Error")).toBeInTheDocument();
  });
});
