import React from "react";
import { describe, it, expect, vi } from "vitest";
import { fireEvent, render, screen } from "@testing-library/react";
import { MovementForm } from "../movement-form";
import { MovementType } from "../../types/movement-type";
import { FrequencyType } from "../../types/frequency-type";

const renderMovementForm = (overrides: Partial<React.ComponentProps<typeof MovementForm>> = {}) =>
  render(
    <MovementForm
      name=""
      amount=""
      type={MovementType.Undefined}
      frequencyType={FrequencyType.Undefined}
      frequencyMonth={1}
      customMonths={[]}
      isEditing={false}
      submitting={false}
      errorMessage={null}
      validationMessage={null}
      onNameChange={vi.fn()}
      onAmountChange={vi.fn()}
      onTypeChange={vi.fn()}
      onFrequencyTypeChange={vi.fn()}
      onFrequencyMonthChange={vi.fn()}
      onCustomMonthsChange={vi.fn()}
      onSubmit={vi.fn()}
      onCancel={vi.fn()}
      {...overrides}
    />,
  );

describe("MovementForm", () => {
  it("renders Spanish labels and button text", () => {
    renderMovementForm();

    expect(screen.getByRole("textbox", { name: /Nombre/i })).toBeInTheDocument();
    expect(screen.getByRole("spinbutton", { name: /Cantidad/i })).toBeInTheDocument();
    expect(screen.getByRole("combobox", { name: /Tipo/i })).toBeInTheDocument();
    expect(screen.getByRole("combobox", { name: /Frecuencia/i })).toBeInTheDocument();
    expect(screen.getByRole("button", { name: "Cancelar" })).toBeInTheDocument();
    expect(
      screen.getByRole("button", { name: "Crear movimiento" }),
    ).toBeInTheDocument();
  });

  it("shows validation and error alerts", () => {
    renderMovementForm({
      validationMessage: "Completa los campos requeridos.",
      errorMessage: "Error",
    });

    expect(screen.getByText("Completa los campos requeridos.")).toBeInTheDocument();
    expect(screen.getByText("Error")).toBeInTheDocument();
  });

  it("disables inputs and shows loading label while submitting", () => {
    renderMovementForm({ submitting: true, isEditing: false });

    expect(screen.getByRole("textbox", { name: /Nombre/i })).toBeDisabled();
    expect(screen.getByRole("spinbutton", { name: /Cantidad/i })).toBeDisabled();
    expect(screen.getByRole("combobox", { name: /Tipo/i })).toHaveAttribute(
      "aria-disabled",
      "true",
    );
    expect(screen.getByRole("combobox", { name: /Frecuencia/i })).toHaveAttribute(
      "aria-disabled",
      "true",
    );
    expect(screen.getByRole("button", { name: "Creando" })).toBeDisabled();
  });

  it("renders editing label when editing", () => {
    renderMovementForm({ isEditing: true });

    expect(
      screen.getByRole("button", { name: "Guardar cambios" }),
    ).toBeInTheDocument();
  });

  it("shows month fields for yearly and custom frequency", () => {
    renderMovementForm({ frequencyType: FrequencyType.Yearly });
    expect(screen.getByRole("combobox", { name: /Mes/i })).toBeInTheDocument();

    renderMovementForm({ frequencyType: FrequencyType.Custom });
    expect(screen.getByRole("combobox", { name: /Meses/i })).toBeInTheDocument();
  });

  it("fires submit and cancel callbacks", () => {
    const onSubmit = vi.fn();
    const onCancel = vi.fn();
    const { container } = renderMovementForm({ onSubmit, onCancel });

    const form = container.querySelector("form");
    if (form) {
      fireEvent.submit(form);
    } else {
      fireEvent.click(screen.getByRole("button", { name: "Crear movimiento" }));
    }

    fireEvent.click(screen.getByRole("button", { name: "Cancelar" }));

    expect(onSubmit).toHaveBeenCalled();
    expect(onCancel).toHaveBeenCalled();
  });
});
