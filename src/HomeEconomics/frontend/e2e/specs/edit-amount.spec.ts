import { expect, test } from "@playwright/test";
import { generateUniqueTestName } from "../fixtures/test-data";
import { apiClient } from "../utils/api-client";
import {
  getOrCreateCurrentMovementMonth,
  inputByTestId,
} from "../utils/e2e-helpers";
import { selectors } from "../utils/selectors";

test.describe("Edit month movement amount", () => {
  let movementId: number | null = null;
  let movementName = "";
  let currentMonthId: number | null = null;
  let monthMovementId: number | null = null;

  test.beforeEach(async () => {
    const currentMonth = await getOrCreateCurrentMovementMonth();
    currentMonthId = currentMonth.id;

    movementName = generateUniqueTestName("e2e-edit");
    const createdMovement = await apiClient.createMovement({
      name: movementName,
      amount: 500,
      type: "expense",
      frequency: "none",
    });
    movementId = createdMovement.id;

    const createdMonthMovement = await apiClient.addMonthMovement(currentMonth.id, {
      movementId: createdMovement.id,
      amount: 500,
    });
    monthMovementId = createdMonthMovement.id;
  });

  test.afterEach(async () => {
    if (currentMonthId !== null && monthMovementId !== null) {
      await apiClient.deleteMonthMovement(currentMonthId, monthMovementId).catch(() => undefined);
    }

    if (movementId !== null) {
      await apiClient.deleteMovement(movementId).catch(() => undefined);
    }
  });

  test("should update month movement amount through dialog", async ({ page }) => {
    await page.goto("/");

    const monthMovementItem = page.locator(selectors.currentMonthList.item(movementName));
    await expect(monthMovementItem).toBeVisible();

    await page.click(selectors.currentMonthList.editAmountButton(movementName));
    await expect(page.locator(selectors.editAmountDialog.container)).toBeVisible();

    await page.fill(inputByTestId(selectors.editAmountDialog.amountInput), "550");
    await page.click(selectors.editAmountDialog.saveButton);

    await expect(page.locator(selectors.editAmountDialog.container)).toHaveCount(0);
    await expect(monthMovementItem).toContainText("550,00");
  });
});
