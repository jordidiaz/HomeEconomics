import { expect, test } from "@playwright/test";
import { generateUniqueTestName } from "../fixtures/test-data";
import { apiClient } from "../utils/api-client";
import { getOrCreateCurrentMovementMonth } from "../utils/e2e-helpers";
import { selectors } from "../utils/selectors";

test.describe("Add movement to current month", () => {
  let movementId: number | null = null;
  let movementName = "";
  let monthMovementId: number | null = null;
  let currentMonthId: number | null = null;

  test.beforeEach(async () => {
    const currentMonth = await getOrCreateCurrentMovementMonth();
    currentMonthId = currentMonth.id;

    movementName = generateUniqueTestName("e2e-add");
    const createdMovement = await apiClient.createMovement({
      name: movementName,
      amount: 1500,
      type: "expense",
      frequency: "none",
    });
    movementId = createdMovement.id;
  });

  test.afterEach(async () => {
    if (currentMonthId !== null && monthMovementId !== null) {
      await apiClient.deleteMonthMovement(currentMonthId, monthMovementId).catch(() => undefined);
    }

    if (movementId !== null) {
      await apiClient.deleteMovement(movementId).catch(() => undefined);
    }
  });

  test("should add a movement from movements list to current month list", async ({ page }) => {
    await page.goto("/");

    await expect(page.locator(selectors.movementsList.item(movementName))).toBeVisible();

    await page.click(selectors.movementsList.addToMonthButton(movementName));

    const monthMovementItem = page.locator(selectors.currentMonthList.item(movementName));
    await expect(monthMovementItem).toBeVisible();
    await expect(monthMovementItem).toContainText("1500,00");

    const currentMonth = await getOrCreateCurrentMovementMonth();
    const monthData = await apiClient.getMovementMonth(currentMonth.year, currentMonth.month);
    if (!monthData) {
      return;
    }

    const created = monthData.monthMovements.at(-1);
    monthMovementId = created?.id ?? null;
  });
});
