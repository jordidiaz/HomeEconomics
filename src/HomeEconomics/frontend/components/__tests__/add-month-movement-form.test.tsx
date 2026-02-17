import React from "react";
import { describe, it, expect, vi } from "vitest";
import { fireEvent, render, screen } from "@testing-library/react";
import { AddMonthMovementForm } from "../add-month-movement-form";
import { MovementType } from "../../types/movement-type";

const renderForm = (overrides: Partial<React.ComponentProps<typeof AddMonthMovementForm>> = {}) =>
  render(
    <AddMonthMovementForm
      name=""
      amount=""
      type={MovementType.Undefined}
      submitting={false}
      errorMessage={null}
      validationMessage={null}
      onNameChange={vi.fn()}
      onAmountChange={vi.fn()}
      onTypeChange={vi.fn()}
      onSubmit={vi.fn()}
      onCancel={vi.fn()}
      {...overrides}
    />,
  );

describe("AddMonthMovementForm", () => {
  it("renders Spanish labels", () => {
    renderForm();

    expect(screen.getByRole("textbox", { name: /Nombre/i })).toBeInTheDocument();
    expect(screen.getByRole("spinbutton", { name: /Cantidad/i })).toBeInTheDocument();
    expect(screen.getByRole("combobox", { name: /Tipo/i })).toBeInTheDocument();
    expect(screen.getByRole("button", { name: "Cancelar" })).toBeInTheDocument();
    expect(screen.getByRole("button", { name: "Aceptar" })).toBeInTheDocument();
  });

  it("shows validation and error alerts", () => {
    renderForm({
      validationMessage: "Completa los campos requeridos.",
      errorMessage: "Error",
    });

    expect(screen.getByText("Completa los campos requeridos.")).toBeInTheDocument();
    expect(screen.getByText("Error")).toBeInTheDocument();
  });

  it("disables inputs and buttons while submitting", () => {
    renderForm({ submitting: true });

    expect(screen.getByRole("textbox", { name: /Nombre/i })).toBeDisabled();
    expect(screen.getByRole("spinbutton", { name: /Cantidad/i })).toBeDisabled();
    expect(screen.getByRole("combobox", { name: /Tipo/i })).toHaveAttribute(
      "aria-disabled",
      "true",
    );
    expect(screen.getByRole("button", { name: "Cancelar" })).toBeDisabled();
    expect(screen.getByRole("button", { name: "Creando" })).toBeDisabled();
  });

  it("fires submit and cancel callbacks", () => {
    const onSubmit = vi.fn();
    const onCancel = vi.fn();
    const { container } = renderForm({ onSubmit, onCancel });

    const form = container.querySelector("form");
    if (form) {
      fireEvent.submit(form);
    } else {
      fireEvent.click(screen.getByRole("button", { name: "Aceptar" }));
    }

    fireEvent.click(screen.getByRole("button", { name: "Cancelar" }));

    expect(onSubmit).toHaveBeenCalled();
    expect(onCancel).toHaveBeenCalled();
  });
});
