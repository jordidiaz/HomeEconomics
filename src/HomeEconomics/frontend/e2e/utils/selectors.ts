export const selectors = {
  movementForm: {
    nameInput: '[data-testid="movement-form-name"]',
    amountInput: '[data-testid="movement-form-amount"]',
    typeSelect: '[data-testid="movement-form-type"]',
    frequencySelect: '[data-testid="movement-form-frequency"]',
    submitButton: '[data-testid="movement-form-submit"]',
    cancelButton: '[data-testid="movement-form-cancel"]',
  },

  movementsList: {
    container: '[data-testid="movements-list"]',
    item: (name: string) => `[data-testid="movement-item-${name}"]`,
    editButton: (name: string) => `[data-testid="movement-edit-${name}"]`,
    deleteButton: (name: string) => `[data-testid="movement-delete-${name}"]`,
    addToMonthButton: (name: string) => `[data-testid="movement-add-to-month-${name}"]`,
  },

  deleteDialog: {
    container: '[data-testid="confirm-delete-movement-dialog"]',
    confirmButton: '[data-testid="delete-movement-confirm"]',
    cancelButton: '[data-testid="delete-movement-cancel"]',
    errorMessage: '[data-testid="delete-movement-error"]',
  },

  monthSelector: {
    previousMonthButton: '[data-testid="month-selector-previous"]',
    currentMonthButton: '[data-testid="month-selector-current"]',
    nextMonthButton: '[data-testid="month-selector-next"]',
    createCurrentMonthButton: '[data-testid="create-current-month"]',
    createNextMonthButton: '[data-testid="create-next-month"]',
  },

  currentMonthList: {
    container: '[data-testid="current-month-movements-list"]',
    item: (name: string) => `[data-testid="month-movement-${name}"]`,
    paidIndicator: (name: string) => `[data-testid="month-movement-paid-${name}"]`,
    payButton: (name: string) => `[data-testid="month-movement-pay-${name}"]`,
    unpayButton: (name: string) => `[data-testid="month-movement-unpay-${name}"]`,
    editAmountButton: (name: string) => `[data-testid="month-movement-edit-amount-${name}"]`,
    deleteButton: (name: string) => `[data-testid="month-movement-delete-${name}"]`,
    moveButton: (name: string) => `[data-testid="month-movement-move-${name}"]`,
  },

  paidToggle: '[data-testid="paid-toggle"]',

  editAmountDialog: {
    container: '[data-testid="edit-amount-dialog"]',
    amountInput: '[data-testid="edit-amount-input"]',
    saveButton: '[data-testid="edit-amount-save"]',
    cancelButton: '[data-testid="edit-amount-cancel"]',
  },

  statusForm: {
    accountInput: '[data-testid="status-account-input"]',
    cashInput: '[data-testid="status-cash-input"]',
    balanceDisplay: '[data-testid="status-balance"]',
    successMessage: '[data-testid="status-success"]',
  },

  addMonthMovementForm: {
    container: '[data-testid="add-month-movement-form"]',
  },
};
