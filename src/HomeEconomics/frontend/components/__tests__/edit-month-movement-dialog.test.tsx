import React from "react";
import { describe, it, expect, vi } from "vitest";
import { fireEvent, render, screen } from "@testing-library/react";
import { EditMonthMovementDialog } from "../edit-month-movement-dialog";
import { MovementType } from "../../types/movement-type";

describe("EditMonthMovementDialog", () => {
  it("renders Spanish labels and fires callbacks", () => {
    const onNameChange = vi.fn();
    const onAmountChange = vi.fn();
    const onTypeChange = vi.fn();
    const onCancel = vi.fn();
    const onAccept = vi.fn();

    render(
      <EditMonthMovementDialog
        open={true}
        name="Luz"
        amount="45"
        type={MovementType.Expense}
        submitting={false}
        errorMessage={null}
        onNameChange={onNameChange}
        onAmountChange={onAmountChange}
        onTypeChange={onTypeChange}
        onCancel={onCancel}
        onAccept={onAccept}
      />,
    );

    expect(screen.getByText("Editar movimiento")).toBeInTheDocument();
    expect(screen.getByLabelText(/Nombre/i)).toBeInTheDocument();
    expect(screen.getByRole("spinbutton", { name: /Cantidad/i })).toBeInTheDocument();
    expect(screen.getByLabelText(/Tipo/i)).toBeInTheDocument();

    fireEvent.change(screen.getByLabelText(/Nombre/i), { target: { value: "Agua" } });
    fireEvent.change(screen.getByRole("spinbutton", { name: /Cantidad/i }), {
      target: { value: "30" },
    });
    fireEvent.click(screen.getByRole("button", { name: "Cancelar" }));
    fireEvent.click(screen.getByRole("button", { name: "Aceptar" }));

    expect(onNameChange).toHaveBeenCalledWith("Agua");
    expect(onAmountChange).toHaveBeenCalledWith("30");
    expect(onCancel).toHaveBeenCalled();
    expect(onAccept).toHaveBeenCalled();
  });

  it("disables all fields and buttons while submitting and shows error", () => {
    render(
      <EditMonthMovementDialog
        open={true}
        name="Luz"
        amount="45"
        type={MovementType.Expense}
        submitting={true}
        errorMessage="Error al guardar"
        onNameChange={vi.fn()}
        onAmountChange={vi.fn()}
        onTypeChange={vi.fn()}
        onCancel={vi.fn()}
        onAccept={vi.fn()}
      />,
    );

    expect(screen.getByLabelText(/Nombre/i)).toBeDisabled();
    expect(screen.getByRole("spinbutton", { name: /Cantidad/i })).toBeDisabled();
    expect(screen.getByRole("button", { name: "Cancelar" })).toBeDisabled();
    expect(screen.getByRole("button", { name: "Aceptar" })).toBeDisabled();
    expect(screen.getByText("Error al guardar")).toBeInTheDocument();
  });
});
