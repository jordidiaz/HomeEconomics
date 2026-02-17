import React from "react";
import { describe, it, expect, vi } from "vitest";
import { render, screen, within } from "@testing-library/react";
import { CurrentMonthMovementsList } from "../current-month-movements-list";
import { MovementType } from "../../types/movement-type";

const createMovements = () => [
  {
    id: 1,
    name: "Seguro",
    amount: "20,00",
    amountValue: 20,
    type: MovementType.Expense,
    typeLabel: "Gasto",
    paid: false,
    paidLabel: "Pendiente",
  },
  {
    id: 2,
    name: "Nomina",
    amount: "1.200,00",
    amountValue: 1200,
    type: MovementType.Income,
    typeLabel: "Ingreso",
    paid: true,
    paidLabel: "Pagado",
  },
];

describe("CurrentMonthMovementsList", () => {
  it("renders paid chip when showPaid is true", () => {
    render(
      <CurrentMonthMovementsList
        movements={createMovements()}
        showPaid={true}
        actionStates={{}}
        amountUpdateState={{ loading: false, monthMovementId: null }}
        deleteState={{ loading: false, monthMovementId: null }}
        moveState={{ loading: false, monthMovementId: null }}
        nextMovementMonthExists={false}
        onPay={vi.fn()}
        onUnpay={vi.fn()}
        onEditAmount={vi.fn()}
        onDelete={vi.fn()}
        onMoveToNextMonth={vi.fn()}
      />,
    );

    expect(screen.getByText("Pagado")).toBeInTheDocument();
  });

  it("hides move action when next month is not available", () => {
    render(
      <CurrentMonthMovementsList
        movements={createMovements()}
        showPaid={false}
        actionStates={{}}
        amountUpdateState={{ loading: false, monthMovementId: null }}
        deleteState={{ loading: false, monthMovementId: null }}
        moveState={{ loading: false, monthMovementId: null }}
        nextMovementMonthExists={false}
        onPay={vi.fn()}
        onUnpay={vi.fn()}
        onEditAmount={vi.fn()}
        onDelete={vi.fn()}
        onMoveToNextMonth={vi.fn()}
      />,
    );

    expect(screen.queryByLabelText("Mover al mes siguiente")).toBeNull();
  });

  it("disables actions when an item is loading", () => {
    render(
      <CurrentMonthMovementsList
        movements={createMovements()}
        showPaid={false}
        actionStates={{
          1: { loading: true, errorMessage: null },
        }}
        amountUpdateState={{ loading: false, monthMovementId: null }}
        deleteState={{ loading: false, monthMovementId: null }}
        moveState={{ loading: false, monthMovementId: null }}
        nextMovementMonthExists={true}
        onPay={vi.fn()}
        onUnpay={vi.fn()}
        onEditAmount={vi.fn()}
        onDelete={vi.fn()}
        onMoveToNextMonth={vi.fn()}
      />,
    );

    const item = screen.getByText("Seguro").closest("li");
    expect(item).not.toBeNull();
    if (item) {
      const editButton = within(item).getByLabelText("Editar importe", {
        selector: "button",
      });
      expect(editButton).toBeDisabled();
    }
  });

  it("shows per-item error messages", () => {
    render(
      <CurrentMonthMovementsList
        movements={createMovements()}
        showPaid={false}
        actionStates={{
          1: { loading: false, errorMessage: "No se pudo actualizar." },
        }}
        amountUpdateState={{ loading: false, monthMovementId: null }}
        deleteState={{ loading: false, monthMovementId: null }}
        moveState={{ loading: false, monthMovementId: null }}
        nextMovementMonthExists={true}
        onPay={vi.fn()}
        onUnpay={vi.fn()}
        onEditAmount={vi.fn()}
        onDelete={vi.fn()}
        onMoveToNextMonth={vi.fn()}
      />,
    );

    expect(screen.getByText("No se pudo actualizar.")).toBeInTheDocument();
  });
});
