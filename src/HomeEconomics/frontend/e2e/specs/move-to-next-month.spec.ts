import { expect, test } from "@playwright/test";
import { generateUniqueTestName } from "../fixtures/test-data";
import { apiClient } from "../utils/api-client";
import {
  getOrCreateCurrentMovementMonth,
  getOrCreateNextMovementMonth,
} from "../utils/e2e-helpers";
import { selectors } from "../utils/selectors";

test.describe("Move month movement to next month", () => {
  let movementId: number | null = null;
  let movementName = "";
  let currentMonthId: number | null = null;
  let monthMovementId: number | null = null;
  let helperMovementId: number | null = null;

  test.beforeEach(async () => {
    // Create a monthly movement first so the backend Create handler finds
    // eligible movements when initialising the next month.
    const helperMovement = await apiClient.createMovement({
      name: generateUniqueTestName("e2e-move"),
      amount: 1,
      type: "expense",
      frequency: "monthly",
    });
    helperMovementId = helperMovement.id;

    const currentMonth = await getOrCreateCurrentMovementMonth();
    currentMonthId = currentMonth.id;
    await getOrCreateNextMovementMonth();

    movementName = generateUniqueTestName("e2e-move");
    const createdMovement = await apiClient.createMovement({
      name: movementName,
      amount: 950,
      type: "expense",
      frequency: "none",
    });
    movementId = createdMovement.id;

    const createdMonthMovement = await apiClient.addMonthMovement(currentMonth.id, {
      movementId: createdMovement.id,
      amount: 950,
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

    if (helperMovementId !== null) {
      await apiClient.deleteMovement(helperMovementId).catch(() => undefined);
    }
  });

  test("should move movement from current to next month", async ({ page }) => {
    await page.goto("/");

    await expect(page.locator(selectors.addMonthMovementForm.container)).toBeVisible();

    const movementInCurrentMonth = page.locator(selectors.currentMonthList.item(movementName));
    await expect(movementInCurrentMonth).toBeVisible();

    await page.click(selectors.currentMonthList.moveButton(movementName));

    const dialog = page.getByRole("dialog");
    await expect(dialog).toContainText("Confirmar acción");
    await dialog.getByRole("button", { name: "Aceptar" }).click();

    await expect(movementInCurrentMonth).toHaveCount(0);

    await page.click(selectors.monthSelector.nextMonthButton);
    await expect(page.locator(selectors.addMonthMovementForm.container)).toBeVisible();
    await expect(page.locator(selectors.currentMonthList.item(movementName))).toBeVisible();
  });
});
