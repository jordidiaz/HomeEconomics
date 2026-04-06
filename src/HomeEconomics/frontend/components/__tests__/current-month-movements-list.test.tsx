import React from "react";
import { describe, it, expect, vi } from "vitest";
import { render, screen, within } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
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
    starred: false,
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
    starred: false,
  },
];

const defaultProps = {
  showPaid: false,
  actionStates: {},
  editState: { loading: false, monthMovementId: null },
  deleteState: { loading: false, monthMovementId: null },
  moveState: { loading: false, monthMovementId: null },
  nextMovementMonthExists: false,
  onPay: vi.fn(),
  onUnpay: vi.fn(),
  onStar: vi.fn(),
  onUnstar: vi.fn(),
  onEdit: vi.fn(),
  onDelete: vi.fn(),
  onMoveToNextMonth: vi.fn(),
};

describe("CurrentMonthMovementsList", () => {
  it("renders paid chip when showPaid is true", () => {
    render(
      <CurrentMonthMovementsList
        {...defaultProps}
        movements={createMovements()}
        showPaid={true}
      />,
    );

    expect(screen.getByText("Pagado")).toBeInTheDocument();
  });

  it("hides move action when next month is not available", () => {
    render(
      <CurrentMonthMovementsList
        {...defaultProps}
        movements={createMovements()}
        nextMovementMonthExists={false}
      />,
    );

    expect(screen.queryByLabelText("Mover al mes siguiente")).toBeNull();
  });

  it("disables actions when an item is loading", () => {
    render(
      <CurrentMonthMovementsList
        {...defaultProps}
        movements={createMovements()}
        actionStates={{ 1: { loading: true, errorMessage: null } }}
        nextMovementMonthExists={true}
      />,
    );

    const item = screen.getByText("Seguro").closest("li");
    expect(item).not.toBeNull();
    if (item) {
      const editButton = within(item).getByLabelText("Editar movimiento", {
        selector: "button",
      });
      expect(editButton).toBeDisabled();
    }
  });

  it("shows per-item error messages", () => {
    render(
      <CurrentMonthMovementsList
        {...defaultProps}
        movements={createMovements()}
        actionStates={{ 1: { loading: false, errorMessage: "No se pudo actualizar." } }}
        nextMovementMonthExists={true}
      />,
    );

    expect(screen.getByText("No se pudo actualizar.")).toBeInTheDocument();
  });

  it("renders star border icon and Destacar label for unstarred movement", () => {
    render(
      <CurrentMonthMovementsList
        {...defaultProps}
        movements={createMovements()}
      />,
    );

    expect(screen.getByTestId("month-movement-star-Seguro")).toBeInTheDocument();
    expect(screen.getAllByLabelText("Destacar", { selector: "button" })).toHaveLength(2);
  });

  it("renders filled star icon and Quitar destacado label for starred movement", () => {
    const movements = createMovements();
    movements[0].starred = true;

    render(
      <CurrentMonthMovementsList
        {...defaultProps}
        movements={movements}
      />,
    );

    expect(screen.getByTestId("month-movement-unstar-Seguro")).toBeInTheDocument();
    expect(screen.getByLabelText("Quitar destacado", { selector: "button" })).toBeInTheDocument();
  });

  it("calls onStar when star button clicked for unstarred movement", async () => {
    const onStar = vi.fn();

    render(
      <CurrentMonthMovementsList
        {...defaultProps}
        movements={createMovements()}
        onStar={onStar}
      />,
    );

    await userEvent.click(screen.getByTestId("month-movement-star-Seguro"));

    expect(onStar).toHaveBeenCalledWith(1);
  });

  it("calls onUnstar when star button clicked for starred movement", async () => {
    const onUnstar = vi.fn();
    const movements = createMovements();
    movements[0].starred = true;

    render(
      <CurrentMonthMovementsList
        {...defaultProps}
        movements={movements}
        onUnstar={onUnstar}
      />,
    );

    await userEvent.click(screen.getByTestId("month-movement-unstar-Seguro"));

    expect(onUnstar).toHaveBeenCalledWith(1);
  });

  it("disables star button when action is loading", () => {
    render(
      <CurrentMonthMovementsList
        {...defaultProps}
        movements={createMovements()}
        actionStates={{ 1: { loading: true, errorMessage: null } }}
      />,
    );

    const starButton = screen.getByTestId("month-movement-star-Seguro");
    expect(starButton).toBeDisabled();
  });
});
