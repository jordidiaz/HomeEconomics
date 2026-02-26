import { expect, test } from "@playwright/test";
import { getCurrentYearMonth } from "../fixtures/test-data";
import { getOrCreateCurrentMovementMonth, inputByTestId } from "../utils/e2e-helpers";
import { selectors } from "../utils/selectors";

type StatusApiResponse = {
  status: {
    pendingTotalExpenses: number;
    pendingTotalIncomes: number;
    accountAmount: number;
    cashAmount: number;
  };
};

function formatAmount(amount: number): string {
  return new Intl.NumberFormat("es-ES", {
    minimumFractionDigits: 2,
    maximumFractionDigits: 2,
  }).format(amount);
}

test.describe("Status form", () => {
  test.beforeEach(async () => {
    await getOrCreateCurrentMovementMonth();
  });

  test("should update account and cash values and show recalculated balance", async ({ page }) => {
    await page.goto("/");

    const accountInput = page.locator(inputByTestId(selectors.statusForm.accountInput));
    const cashInput = page.locator(inputByTestId(selectors.statusForm.cashInput));

    await accountInput.fill("5000");
    await accountInput.press("Tab");

    await cashInput.fill("200");
    await cashInput.press("Tab");

    await expect(accountInput).toHaveValue("5000");
    await expect(cashInput).toHaveValue("200");

    const { year, month } = getCurrentYearMonth();

    let resolvedStatus: StatusApiResponse["status"] | null = null;
    for (let attempt = 0; attempt < 20; attempt++) {
      const response = await fetch(
        `http://localhost:5000/api/movement-months/${year}/${month}`,
      );

      if (response.ok) {
        const data = (await response.json()) as StatusApiResponse;
        if (
          data.status.accountAmount === 5000 &&
          data.status.cashAmount === 200
        ) {
          resolvedStatus = data.status;
          break;
        }
      }

      await page.waitForTimeout(300);
    }

    expect(resolvedStatus).not.toBeNull();

    const status = resolvedStatus!;
    const expectedBalance =
      status.accountAmount +
      status.cashAmount -
      (status.pendingTotalExpenses - status.pendingTotalIncomes);

    await expect(page.locator(selectors.statusForm.balanceDisplay)).toContainText(
      formatAmount(expectedBalance),
    );
  });
});
