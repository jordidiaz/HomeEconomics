import React from "react";
import { describe, it, expect, vi } from "vitest";
import { act, render, screen } from "@testing-library/react";
import { MovementsList } from "../movements-list";
import { MovementType } from "../../types/movement-type";

const createMovements = () => [
  {
    id: 1,
    name: "Nomina",
    amount: "1.200,00",
    type: MovementType.Income,
    typeLabel: "Ingreso",
    frequencyLabel: "Mensual",
  },
  {
    id: 2,
    name: "Seguro",
    amount: "20,00",
    type: MovementType.Expense,
    typeLabel: "Gasto",
    frequencyLabel: "Único",
  },
];

describe("MovementsList", () => {
  it("renders movement data and Spanish labels", () => {
    render(
      <MovementsList
        movements={createMovements()}
        deleting={false}
        addDisabled={false}
        addActionStates={{}}
        onAddToCurrentMonth={vi.fn()}
        onDeleteRequest={vi.fn()}
        onEditRequest={vi.fn()}
      />,
    );

    expect(screen.getByText("Nomina")).toBeInTheDocument();
    expect(screen.getByText("Mensual")).toBeInTheDocument();
    expect(screen.getByText("Ingreso")).toBeInTheDocument();
    expect(screen.getByText("1.200,00")).toBeInTheDocument();

    expect(screen.getByText("Seguro")).toBeInTheDocument();
    expect(screen.getByText("Único")).toBeInTheDocument();
    expect(screen.getByText("Gasto")).toBeInTheDocument();
    expect(screen.getByText("20,00")).toBeInTheDocument();
  });

  it("disables action buttons based on flags", () => {
    const onAddToCurrentMonth = vi.fn();
    const onDeleteRequest = vi.fn();
    const onEditRequest = vi.fn();
    render(
      <MovementsList
        movements={createMovements()}
        deleting={true}
        addDisabled={true}
        addActionStates={{
          1: { loading: true, errorMessage: null },
        }}
        onAddToCurrentMonth={onAddToCurrentMonth}
        onDeleteRequest={onDeleteRequest}
        onEditRequest={onEditRequest}
      />,
    );

    const addButtons = screen.getAllByRole("button", {
      name: "Agregar al mes actual",
    });
    const editButtons = screen.getAllByRole("button", { name: "Editar" });
    const deleteButtons = screen.getAllByRole("button", { name: "Eliminar" });

    act(() => {
      addButtons.forEach((button) => button.click());
      editButtons.forEach((button) => button.click());
      deleteButtons.forEach((button) => button.click());
    });

    expect(onAddToCurrentMonth).not.toHaveBeenCalled();
    expect(onEditRequest).not.toHaveBeenCalled();
    expect(onDeleteRequest).not.toHaveBeenCalled();
  });

  it("shows per-item error messages", () => {
    render(
      <MovementsList
        movements={createMovements()}
        deleting={false}
        addDisabled={false}
        addActionStates={{
          2: {
            loading: false,
            errorMessage: "No se pudo agregar el movimiento.",
          },
        }}
        onAddToCurrentMonth={vi.fn()}
        onDeleteRequest={vi.fn()}
        onEditRequest={vi.fn()}
      />,
    );

    expect(screen.getByText("No se pudo agregar el movimiento.")).toBeInTheDocument();
  });
});
