import React from "react";
import { describe, it, expect, vi } from "vitest";
import { fireEvent, render, screen } from "@testing-library/react";
import { MonthMovementMonthSelector } from "../month-movement-month-selector";

const baseProps: React.ComponentProps<typeof MonthMovementMonthSelector> = {
  currentMonth: { year: 2024, month: 5 },
  nextMonth: { year: 2024, month: 6 },
  selectedMonth: "current",
  nextMonthAvailable: true,
  disabled: false,
  creatingNextMonth: false,
  createNextMonthErrorMessage: null,
  creatingCurrentMonth: false,
  createCurrentMonthErrorMessage: null,
  showCreateMonth: false,
  showCreateNextMonth: false,
  onSelect: vi.fn(),
  onCreateNextMonth: vi.fn(),
  onCreateMonth: vi.fn(),
};

describe("MonthMovementMonthSelector", () => {
  it("renders current and next month labels", () => {
    render(<MonthMovementMonthSelector {...baseProps} />);

    expect(screen.getByText("Mayo 2024")).toBeInTheDocument();
    expect(screen.getByText("Junio 2024")).toBeInTheDocument();
  });

  it("disables next month toggle when not available", () => {
    render(
      <MonthMovementMonthSelector
        {...baseProps}
        nextMonthAvailable={false}
        selectedMonth="current"
      />,
    );

    const nextToggle = screen.getByRole("button", { name: "Junio 2024" });
    expect(nextToggle).toBeDisabled();
  });

  it("shows create month buttons and errors", () => {
    render(
      <MonthMovementMonthSelector
        {...baseProps}
        showCreateMonth={true}
        showCreateNextMonth={true}
        createCurrentMonthErrorMessage="Error actual"
        createNextMonthErrorMessage="Error siguiente"
      />,
    );

    expect(screen.getByRole("button", { name: "Crear mes actual" })).toBeInTheDocument();
    expect(
      screen.getByRole("button", { name: "Crear mes siguiente" }),
    ).toBeInTheDocument();
    expect(screen.getByText("Error actual")).toBeInTheDocument();
    expect(screen.getByText("Error siguiente")).toBeInTheDocument();
  });

  it("fires callbacks for selection and create actions", () => {
    const onSelect = vi.fn();
    const onCreateMonth = vi.fn();
    const onCreateNextMonth = vi.fn();

    render(
      <MonthMovementMonthSelector
        {...baseProps}
        onSelect={onSelect}
        onCreateMonth={onCreateMonth}
        onCreateNextMonth={onCreateNextMonth}
        showCreateMonth={true}
        showCreateNextMonth={true}
      />,
    );

    fireEvent.click(screen.getByRole("button", { name: "Junio 2024" }));
    fireEvent.click(screen.getByRole("button", { name: "Crear mes actual" }));
    fireEvent.click(screen.getByRole("button", { name: "Crear mes siguiente" }));

    expect(onSelect).toHaveBeenCalled();
    expect(onCreateMonth).toHaveBeenCalled();
    expect(onCreateNextMonth).toHaveBeenCalled();
  });
});
