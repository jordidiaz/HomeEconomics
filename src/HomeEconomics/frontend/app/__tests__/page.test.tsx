import React from "react";
import { fireEvent, render, screen, waitFor } from "@testing-library/react";
import { beforeEach, describe, expect, it, vi } from "vitest";
import HomePage from "../page";
import { FrequencyType } from "../../types/frequency-type";
import type { Movement } from "../../types/movement";
import { MovementType } from "../../types/movement-type";

type UseMovementFormOptions = {
  onSaved?: () => Promise<void> | void;
};

type UseDeleteMovementOptions = {
  onDeleted?: () => Promise<void> | void;
};

type UseAddMovementToCurrentMonthOptions = {
  movementMonthId: number | null;
  onAdded?: () => Promise<void> | void;
};

const {
  useMovementsMock,
  useMovementFormMock,
  useDeleteMovementMock,
  useCurrentMonthMovementsMock,
  useAddMovementToCurrentMonthMock,
  useAddMonthMovementFormMock,
  useMovementMonthStatusFormMock,
} = vi.hoisted(() => ({
  useMovementsMock: vi.fn(),
  useMovementFormMock: vi.fn(),
  useDeleteMovementMock: vi.fn(),
  useCurrentMonthMovementsMock: vi.fn(),
  useAddMovementToCurrentMonthMock: vi.fn(),
  useAddMonthMovementFormMock: vi.fn(),
  useMovementMonthStatusFormMock: vi.fn(),
}));

vi.mock("../../hooks/use-movements", () => ({
  useMovements: useMovementsMock,
}));

vi.mock("../../hooks/use-movement-form", () => ({
  useMovementForm: useMovementFormMock,
}));

vi.mock("../../hooks/use-delete-movement", () => ({
  useDeleteMovement: useDeleteMovementMock,
}));

vi.mock("../../hooks/use-current-month-movements", () => ({
  useCurrentMonthMovements: useCurrentMonthMovementsMock,
}));

vi.mock("../../hooks/use-add-movement-to-current-month", () => ({
  useAddMovementToCurrentMonth: useAddMovementToCurrentMonthMock,
}));

vi.mock("../../hooks/use-add-month-movement-form", () => ({
  useAddMonthMovementForm: useAddMonthMovementFormMock,
}));

vi.mock("../../hooks/use-movement-month-status-form", () => ({
  useMovementMonthStatusForm: useMovementMonthStatusFormMock,
}));

vi.mock("../../components/movement-form", () => ({
  MovementForm: (props: {
    onSubmit: () => Promise<void>;
  }) => (
    <button type="button" onClick={() => void props.onSubmit()}>
      submit movement
    </button>
  ),
}));

vi.mock("../../components/movements-list", () => ({
  MovementsList: (props: {
    onEditRequest: (id: number) => void;
    onDeleteRequest: (id: number, name: string) => void;
    onAddToCurrentMonth: (id: number) => void;
  }) => (
    <>
      <button type="button" onClick={() => props.onEditRequest(1)}>
        request edit movement
      </button>
      <button type="button" onClick={() => props.onDeleteRequest(1, "Nomina")}>
        request delete movement
      </button>
      <button type="button" onClick={() => props.onAddToCurrentMonth(1)}>
        add movement to current month
      </button>
    </>
  ),
}));

vi.mock("../../components/month-movement-month-selector", () => ({
  MonthMovementMonthSelector: (props: {
    onSelect: (value: "previous" | "current" | "next") => void;
  }) => (
    <>
      <button type="button" onClick={() => props.onSelect("previous")}>
        select previous month
      </button>
      <button type="button" onClick={() => props.onSelect("current")}>
        select current month
      </button>
      <button type="button" onClick={() => props.onSelect("next")}>
        select next month
      </button>
    </>
  ),
}));

vi.mock("../../components/current-month-movements-list", () => ({
  CurrentMonthMovementsList: (props: {
    showPaid: boolean;
    onEditAmount: (movement: { id: number; name: string; amountValue: number }) => void;
    onDelete: (movementId: number) => void;
    onMoveToNextMonth: (movementId: number) => void;
  }) => (
    <>
      <div>show paid: {props.showPaid ? "yes" : "no"}</div>
      <button
        type="button"
        onClick={() => props.onEditAmount({ id: 10, name: "Luz", amountValue: 45 })}
      >
        request edit month movement amount
      </button>
      <button type="button" onClick={() => props.onDelete(10)}>
        request delete month movement
      </button>
      <button type="button" onClick={() => props.onMoveToNextMonth(10)}>
        request move month movement
      </button>
    </>
  ),
}));

vi.mock("../../components/confirm-delete-movement-dialog", () => ({
  ConfirmDeleteMovementDialog: (props: {
    open: boolean;
    onConfirm: () => Promise<void>;
    onCancel: () => void;
  }) =>
    props.open ? (
      <div>
        <button type="button" onClick={() => void props.onConfirm()}>
          confirm delete movement
        </button>
        <button type="button" onClick={props.onCancel}>
          cancel delete movement
        </button>
      </div>
    ) : null,
}));

vi.mock("../../components/edit-month-movement-amount-dialog", () => ({
  EditMonthMovementAmountDialog: (props: {
    open: boolean;
    onAccept: () => Promise<void>;
    onCancel: () => void;
  }) =>
    props.open ? (
      <div>
        <button type="button" onClick={() => void props.onAccept()}>
          confirm edit month movement amount
        </button>
        <button type="button" onClick={props.onCancel}>
          cancel edit month movement amount
        </button>
      </div>
    ) : null,
}));

vi.mock("../../components/confirm-delete-month-movement-dialog", () => ({
  ConfirmDeleteMonthMovementDialog: (props: {
    open: boolean;
    onConfirm: () => Promise<void>;
    onCancel: () => void;
  }) =>
    props.open ? (
      <div>
        <button type="button" onClick={() => void props.onConfirm()}>
          confirm delete month movement
        </button>
        <button type="button" onClick={props.onCancel}>
          cancel delete month movement
        </button>
      </div>
    ) : null,
}));

vi.mock("../../components/confirm-move-month-movement-dialog", () => ({
  ConfirmMoveMonthMovementDialog: (props: {
    open: boolean;
    onConfirm: () => Promise<void>;
    onCancel: () => void;
  }) =>
    props.open ? (
      <div>
        <button type="button" onClick={() => void props.onConfirm()}>
          confirm move month movement
        </button>
        <button type="button" onClick={props.onCancel}>
          cancel move month movement
        </button>
      </div>
    ) : null,
}));

vi.mock("../../components/movement-month-status-form", () => ({
  MovementMonthStatusForm: () => null,
}));

vi.mock("../../components/add-month-movement-form", () => ({
  AddMonthMovementForm: () => <div>add month movement form</div>,
}));

const movement: Movement = {
  id: 1,
  name: "Nomina",
  amount: 1200,
  type: MovementType.Income,
  frequencyType: FrequencyType.Monthly,
  frequencyMonth: 1,
  frequencyMonths: Array(12).fill(false),
};

const monthMovement = {
  id: 10,
  name: "Luz",
  amount: "45,00",
  amountValue: 45,
  type: MovementType.Expense,
  typeLabel: "Gasto",
  paid: false,
  paidLabel: "Pendiente",
};

const createCurrentMonthMovementsMockValue = (
  overrides: Partial<Record<string, unknown>> = {},
) => ({
  currentMovementMonthId: 77,
  reloadCurrentMonthMovements: vi.fn().mockResolvedValue(undefined),
  reloadSelectedMonthMovements: vi.fn().mockResolvedValue(undefined),
  movementMonth: { id: 77, year: 2026, month: 2 },
  status: null,
  monthMovements: [monthMovement],
  loading: false,
  error: null,
  currentMonth: { year: 2026, month: 2 },
  nextMonth: { year: 2026, month: 3 },
  previousMonth: { year: 2026, month: 1 },
  selectedMonth: "current",
  nextMonthAvailable: true,
  previousMonthAvailable: false,
  creatingNextMonth: false,
  createNextMonthErrorMessage: null,
  creatingCurrentMonth: false,
  createCurrentMonthErrorMessage: null,
  currentMonthAvailable: true,
  movementMonthLoaded: true,
  totalMonthMovements: 1,
  showPaid: false,
  setShowPaid: vi.fn(),
  selectMonth: vi.fn(),
  createNextMonth: vi.fn(),
  createCurrentMonth: vi.fn(),
  actionStates: {},
  amountUpdateState: { loading: false, errorMessage: null, monthMovementId: null },
  deleteState: { loading: false, errorMessage: null, monthMovementId: null },
  moveState: { loading: false, errorMessage: null, monthMovementId: null },
  nextMovementMonthExists: true,
  payMonthMovement: vi.fn(),
  unpayMonthMovement: vi.fn(),
  setDeleteTarget: vi.fn(),
  deleteMonthMovement: vi.fn(async () => true),
  setMoveTarget: vi.fn(),
  moveMonthMovementToNextMonth: vi.fn(async () => true),
  setAmountUpdateTarget: vi.fn(),
  updateMonthMovementAmount: vi.fn(async () => true),
  ...overrides,
});

const createMovementsMockValue = (reload = vi.fn().mockResolvedValue(undefined)) => ({
  movements: [
    {
      id: 1,
      name: "Nomina",
      amount: "1200,00",
      type: MovementType.Income,
      typeLabel: "Ingreso",
      frequencyLabel: "Mensual",
    },
  ],
  movementMap: { 1: movement },
  loading: false,
  error: null,
  reload,
});

const mockMovementFormWithOnSaved = (startEdit = vi.fn()) => {
  useMovementFormMock.mockImplementation((options: UseMovementFormOptions = {}) => ({
    name: "",
    amount: "",
    type: MovementType.Undefined,
    frequencyType: FrequencyType.Undefined,
    frequencyMonth: 1,
    customMonths: [],
    isEditing: false,
    submitting: false,
    errorMessage: null,
    validationMessage: null,
    setName: vi.fn(),
    setAmount: vi.fn(),
    setType: vi.fn(),
    setFrequencyType: vi.fn(),
    setFrequencyMonth: vi.fn(),
    setCustomMonths: vi.fn(),
    submit: async () => {
      if (options.onSaved) {
        await options.onSaved();
      }
    },
    startEdit,
    cancel: vi.fn(),
  }));
};

describe("HomePage integration wiring", () => {
  beforeEach(() => {
    Object.assign(globalThis, { React });
    vi.clearAllMocks();

    useMovementsMock.mockReturnValue(createMovementsMockValue());
    mockMovementFormWithOnSaved();
    useDeleteMovementMock.mockImplementation(
      (options: UseDeleteMovementOptions = {}) => ({
        deletingId: null,
        deleting: false,
        errorMessage: null,
        deleteMovement: async () => {
          if (options.onDeleted) {
            await options.onDeleted();
          }
          return true;
        },
        clearError: vi.fn(),
      }),
    );
    useCurrentMonthMovementsMock.mockReturnValue(createCurrentMonthMovementsMockValue());
    useAddMovementToCurrentMonthMock.mockImplementation(
      (options: UseAddMovementToCurrentMonthOptions) => ({
        actionStates: {},
        addToCurrentMonth: async () => {
          if (options.onAdded) {
            await options.onAdded();
          }
        },
      }),
    );
    useAddMonthMovementFormMock.mockReturnValue({
      name: "",
      amount: "",
      type: MovementType.Undefined,
      submitting: false,
      errorMessage: null,
      validationMessage: null,
      setName: vi.fn(),
      setAmount: vi.fn(),
      setType: vi.fn(),
      submit: vi.fn(),
      cancel: vi.fn(),
    });
    useMovementMonthStatusFormMock.mockReturnValue({
      accountAmount: "",
      cashAmount: "",
      balance: 0,
      submitting: false,
      errorMessage: null,
      setAccountAmount: vi.fn(),
      setCashAmount: vi.fn(),
      submitOnBlur: vi.fn(),
    });
  });

  it("triggers movements reload after create save", async () => {
    const reload = vi.fn().mockResolvedValue(undefined);
    useMovementsMock.mockReturnValue(createMovementsMockValue(reload));

    render(<HomePage />);

    fireEvent.click(screen.getByRole("button", { name: "submit movement" }));

    await waitFor(() => {
      expect(reload).toHaveBeenCalledTimes(1);
    });
  });

  it("wires edit selection and reload after edit save", async () => {
    const reload = vi.fn().mockResolvedValue(undefined);
    const startEdit = vi.fn();
    useMovementsMock.mockReturnValue(createMovementsMockValue(reload));
    mockMovementFormWithOnSaved(startEdit);

    render(<HomePage />);

    fireEvent.click(screen.getByRole("button", { name: "request edit movement" }));
    expect(startEdit).toHaveBeenCalledWith(movement);

    fireEvent.click(screen.getByRole("button", { name: "submit movement" }));

    await waitFor(() => {
      expect(reload).toHaveBeenCalledTimes(1);
    });
  });

  it("triggers movements reload after delete confirm", async () => {
    const reload = vi.fn().mockResolvedValue(undefined);
    const deleteMovement = vi.fn(async () => true);
    useMovementsMock.mockReturnValue(createMovementsMockValue(reload));
    useDeleteMovementMock.mockImplementation(
      (options: UseDeleteMovementOptions = {}) => ({
        deletingId: null,
        deleting: false,
        errorMessage: null,
        deleteMovement: async (id: number) => {
          const wasDeleted = await deleteMovement(id);
          if (wasDeleted && options.onDeleted) {
            await options.onDeleted();
          }
          return wasDeleted;
        },
        clearError: vi.fn(),
      }),
    );

    render(<HomePage />);

    fireEvent.click(screen.getByRole("button", { name: "request delete movement" }));
    fireEvent.click(screen.getByRole("button", { name: "confirm delete movement" }));

    await waitFor(() => {
      expect(deleteMovement).toHaveBeenCalledWith(1);
      expect(reload).toHaveBeenCalledTimes(1);
    });
  });

  it("triggers current month reload after adding movement to current month", async () => {
    const reloadCurrentMonthMovements = vi.fn().mockResolvedValue(undefined);
    useCurrentMonthMovementsMock.mockReturnValue(
      createCurrentMonthMovementsMockValue({
        reloadCurrentMonthMovements,
      }),
    );

    render(<HomePage />);

    fireEvent.click(
      screen.getByRole("button", { name: "add movement to current month" }),
    );

    await waitFor(() => {
      expect(reloadCurrentMonthMovements).toHaveBeenCalledTimes(1);
    });
  });

  it("wires selector switching through selectMonth", () => {
    const selectMonth = vi.fn();
    useCurrentMonthMovementsMock.mockReturnValue(
      createCurrentMonthMovementsMockValue({
        selectMonth,
      }),
    );

    render(<HomePage />);

    fireEvent.click(screen.getByRole("button", { name: "select next month" }));
    fireEvent.click(screen.getByRole("button", { name: "select current month" }));

    expect(selectMonth).toHaveBeenNthCalledWith(1, "next");
    expect(selectMonth).toHaveBeenNthCalledWith(2, "current");
  });

  it("wires selector switching for previous month", () => {
    const selectMonth = vi.fn();
    useCurrentMonthMovementsMock.mockReturnValue(
      createCurrentMonthMovementsMockValue({
        selectMonth,
      }),
    );

    render(<HomePage />);

    fireEvent.click(screen.getByRole("button", { name: "select previous month" }));

    expect(selectMonth).toHaveBeenCalledWith("previous");
  });

  it("shows add month movement form when current month is selected", () => {
    useCurrentMonthMovementsMock.mockReturnValue(
      createCurrentMonthMovementsMockValue({ selectedMonth: "current" }),
    );

    render(<HomePage />);

    expect(screen.getByText("add month movement form")).toBeInTheDocument();
  });

  it("shows add month movement form when next month is selected", () => {
    useCurrentMonthMovementsMock.mockReturnValue(
      createCurrentMonthMovementsMockValue({ selectedMonth: "next" }),
    );

    render(<HomePage />);

    expect(screen.getByText("add month movement form")).toBeInTheDocument();
  });

  it("wires add month movement form to selected month id", () => {
    useCurrentMonthMovementsMock.mockReturnValue(
      createCurrentMonthMovementsMockValue({
        currentMovementMonthId: 77,
        movementMonth: { id: 88, year: 2026, month: 3 },
      }),
    );

    render(<HomePage />);

    expect(useAddMonthMovementFormMock).toHaveBeenCalledWith({
      movementMonthId: 88,
      onAdded: expect.any(Function),
    });
  });

  it("wires paid toggle through setShowPaid", () => {
    const setShowPaid = vi.fn();
    useCurrentMonthMovementsMock.mockReturnValue(
      createCurrentMonthMovementsMockValue({
        setShowPaid,
      }),
    );

    render(<HomePage />);

    fireEvent.click(screen.getByRole("checkbox", { name: "Mostrar pagados" }));

    expect(setShowPaid).toHaveBeenCalledWith(true);
  });

  it("calls update amount and closes edit amount dialog on success", async () => {
    const updateMonthMovementAmount = vi.fn(async () => true);
    useCurrentMonthMovementsMock.mockReturnValue(
      createCurrentMonthMovementsMockValue({
        updateMonthMovementAmount,
      }),
    );

    render(<HomePage />);

    fireEvent.click(
      screen.getByRole("button", { name: "request edit month movement amount" }),
    );
    expect(
      screen.getByRole("button", { name: "confirm edit month movement amount" }),
    ).toBeInTheDocument();

    fireEvent.click(
      screen.getByRole("button", { name: "confirm edit month movement amount" }),
    );

    await waitFor(() => {
      expect(updateMonthMovementAmount).toHaveBeenCalledWith(10, "45.00");
      expect(
        screen.queryByRole("button", { name: "confirm edit month movement amount" }),
      ).not.toBeInTheDocument();
    });
  });

  it("calls delete month movement and closes delete dialog on success", async () => {
    const deleteMonthMovement = vi.fn(async () => true);
    useCurrentMonthMovementsMock.mockReturnValue(
      createCurrentMonthMovementsMockValue({
        deleteMonthMovement,
      }),
    );

    render(<HomePage />);

    fireEvent.click(screen.getByRole("button", { name: "request delete month movement" }));
    expect(
      screen.getByRole("button", { name: "confirm delete month movement" }),
    ).toBeInTheDocument();

    fireEvent.click(screen.getByRole("button", { name: "confirm delete month movement" }));

    await waitFor(() => {
      expect(deleteMonthMovement).toHaveBeenCalledWith(10);
      expect(
        screen.queryByRole("button", { name: "confirm delete month movement" }),
      ).not.toBeInTheDocument();
    });
  });

  it("calls move month movement and closes move dialog on success", async () => {
    const moveMonthMovementToNextMonth = vi.fn(async () => true);
    useCurrentMonthMovementsMock.mockReturnValue(
      createCurrentMonthMovementsMockValue({
        moveMonthMovementToNextMonth,
      }),
    );

    render(<HomePage />);

    fireEvent.click(screen.getByRole("button", { name: "request move month movement" }));
    expect(
      screen.getByRole("button", { name: "confirm move month movement" }),
    ).toBeInTheDocument();

    fireEvent.click(screen.getByRole("button", { name: "confirm move month movement" }));

    await waitFor(() => {
      expect(moveMonthMovementToNextMonth).toHaveBeenCalledWith(10);
      expect(
        screen.queryByRole("button", { name: "confirm move month movement" }),
      ).not.toBeInTheDocument();
    });
  });
});
