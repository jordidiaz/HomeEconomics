import { expect, test } from "@playwright/test";
import { generateUniqueTestName } from "../fixtures/test-data";
import { apiClient } from "../utils/api-client";
import { getOrCreateCurrentMovementMonth } from "../utils/e2e-helpers";
import { selectors } from "../utils/selectors";

test.describe("Pay and unpay", () => {
  let movementId: number | null = null;
  let movementName = "";
  let currentMonthId: number | null = null;
  let monthMovementId: number | null = null;

  test.beforeEach(async () => {
    const currentMonth = await getOrCreateCurrentMovementMonth();
    currentMonthId = currentMonth.id;

    movementName = generateUniqueTestName("e2e-pay");
    const createdMovement = await apiClient.createMovement({
      name: movementName,
      amount: 800,
      type: "expense",
      frequency: "none",
    });
    movementId = createdMovement.id;

    const createdMonthMovement = await apiClient.addMonthMovement(currentMonth.id, {
      movementId: createdMovement.id,
      amount: 800,
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

  test("should pay and unpay movement and respect paid filter", async ({ page }) => {
    await page.goto("/");

    const monthMovementItem = page.locator(selectors.currentMonthList.item(movementName));
    await expect(monthMovementItem).toBeVisible();

    await page.click(selectors.currentMonthList.payButton(movementName));
    await expect(monthMovementItem).toHaveCount(0);

    await page.click(selectors.paidToggle);

    await expect(monthMovementItem).toBeVisible();
    await expect(page.locator(selectors.currentMonthList.paidIndicator(movementName))).toBeVisible();

    await page.click(selectors.currentMonthList.unpayButton(movementName));
    await expect(monthMovementItem).toHaveCount(0);

    await page.click(selectors.paidToggle);
    await expect(monthMovementItem).toBeVisible();
  });
});
