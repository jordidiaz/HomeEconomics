import { expect, test } from "@playwright/test";
import { selectors } from "../utils/selectors";

test.describe("Month setup", () => {
  test("should create current month when missing and load month area", async ({ page }) => {
    await page.goto("/");

    const createCurrentMonthButton = page.locator(
      selectors.monthSelector.createCurrentMonthButton,
    );
    if (await createCurrentMonthButton.isVisible()) {
      await createCurrentMonthButton.click();
    }

    await expect(
      page.getByText(
        "No se pudieron cargar los movimientos del mes. Por favor, inténtalo de nuevo.",
      ),
    ).toHaveCount(0);

    const currentMonthList = page.locator(selectors.currentMonthList.container);
    const currentMonthEmpty = page.getByText(
      "No hay movimientos registrados para el mes actual.",
    );
    const currentMonthNoPending = page.getByText(
      "No hay movimientos pendientes para el mes actual.",
    );
    const currentMonthNoPaid = page.getByText(
      "No hay movimientos pagados para el mes actual.",
    );

    await expect
      .poll(async () => {
        if ((await currentMonthList.count()) > 0) {
          return "list";
        }
        if ((await currentMonthEmpty.count()) > 0) {
          return "empty";
        }
        if ((await currentMonthNoPending.count()) > 0) {
          return "no-pending";
        }
        if ((await currentMonthNoPaid.count()) > 0) {
          return "no-paid";
        }
        return "loading";
      })
      .not.toBe("loading");
  });
});
