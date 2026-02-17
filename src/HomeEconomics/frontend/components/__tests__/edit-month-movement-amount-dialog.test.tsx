import React from "react";
import { describe, it, expect, vi } from "vitest";
import { fireEvent, render, screen } from "@testing-library/react";
import { EditMonthMovementAmountDialog } from "../edit-month-movement-amount-dialog";

describe("EditMonthMovementAmountDialog", () => {
  it("renders inputs and fires callbacks", () => {
    const onAmountChange = vi.fn();
    const onCancel = vi.fn();
    const onAccept = vi.fn();

    render(
      <EditMonthMovementAmountDialog
        open={true}
        amount="10"
        submitting={false}
        errorMessage={null}
        onAmountChange={onAmountChange}
        onCancel={onCancel}
        onAccept={onAccept}
      />,
    );

    expect(screen.getByText("Editar importe")).toBeInTheDocument();
    const input = screen.getByRole("spinbutton", { name: /Cantidad/i });
    fireEvent.change(input, { target: { value: "20" } });
    fireEvent.click(screen.getByRole("button", { name: "Cancelar" }));
    fireEvent.click(screen.getByRole("button", { name: "Aceptar" }));

    expect(onAmountChange).toHaveBeenCalledWith("20");
    expect(onCancel).toHaveBeenCalled();
    expect(onAccept).toHaveBeenCalled();
  });

  it("disables while submitting and shows error", () => {
    render(
      <EditMonthMovementAmountDialog
        open={true}
        amount="10"
        submitting={true}
        errorMessage="Error"
        onAmountChange={vi.fn()}
        onCancel={vi.fn()}
        onAccept={vi.fn()}
      />,
    );

    expect(screen.getByRole("spinbutton", { name: /Cantidad/i })).toBeDisabled();
    expect(screen.getByRole("button", { name: "Cancelar" })).toBeDisabled();
    expect(screen.getByRole("button", { name: "Aceptar" })).toBeDisabled();
    expect(screen.getByText("Error")).toBeInTheDocument();
  });
});
