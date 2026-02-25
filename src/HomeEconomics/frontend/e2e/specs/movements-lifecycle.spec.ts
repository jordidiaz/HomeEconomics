import { expect, test } from "@playwright/test";
import { generateUniqueTestName } from "../fixtures/test-data";
import { apiClient } from "../utils/api-client";
import { selectors } from "../utils/selectors";

type MovementListResponse = {
  movements: Array<{
    id: number;
    name: string;
  }>;
};

async function deleteMovementsByName(name: string): Promise<void> {
  const response = await fetch("http://localhost:5000/api/movements");
  if (!response.ok) {
    return;
  }

  const data = (await response.json()) as MovementListResponse;
  const matchingMovements = data.movements.filter((movement) => movement.name === name);

  await Promise.all(
    matchingMovements.map((movement) =>
      apiClient.deleteMovement(movement.id).catch(() => undefined),
    ),
  );
}

test.describe("Movements lifecycle", () => {
  let movementName = "";

  test.afterEach(async () => {
    if (!movementName) {
      return;
    }

    await deleteMovementsByName(movementName);
  });

  test("should create, edit, and delete a movement", async ({ page }) => {
    movementName = generateUniqueTestName("E2E Movimiento");

    await page.goto("/");

    await page.fill(inputByTestId(selectors.movementForm.nameInput), movementName);
    await page.fill(inputByTestId(selectors.movementForm.amountInput), "1200");

    await page.locator(selectors.movementForm.typeSelect).click();
    await page.getByRole("option", { name: "Gasto" }).click();

    await page.locator(selectors.movementForm.frequencySelect).click();
    await page.getByRole("option", { name: "Mensual" }).click();

    await page.click(selectors.movementForm.submitButton);

    const movementItem = page.locator(selectors.movementsList.item(movementName));
    await expect(movementItem).toBeVisible();
    await expect(movementItem).toContainText("1200,00");

    await page.click(selectors.movementsList.editButton(movementName));
    await page.fill(inputByTestId(selectors.movementForm.amountInput), "1250");
    await page.click(selectors.movementForm.submitButton);

    await expect(movementItem).toContainText("1250,00");

    await page.click(selectors.movementsList.deleteButton(movementName));
    await expect(page.locator(selectors.deleteDialog.container)).toBeVisible();
    await expect(page.locator(selectors.deleteDialog.container)).toContainText(movementName);

    await page.click(selectors.deleteDialog.confirmButton);
    await expect(page.locator(selectors.movementsList.item(movementName))).toHaveCount(0);
  });
});
const inputByTestId = (testIdSelector: string): string => `${testIdSelector} input`;
