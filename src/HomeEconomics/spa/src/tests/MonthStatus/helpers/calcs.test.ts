import { getMovementMonth } from "../../builders/movement-months";
import { calculateRemaining } from "../../../App/MonthStatus/helpers/calcs";

describe('calcs', () => {

  describe('calculateRemaining', () => {

    const movementMonth = getMovementMonth();

    test('should return the remaining', () => {
      expect(calculateRemaining(movementMonth, 300, 10)).toBe(200);
      expect(calculateRemaining(movementMonth, NaN, 10)).toBe(-100);
      expect(calculateRemaining(movementMonth, 300, NaN)).toBe(190);
      expect(calculateRemaining(movementMonth, NaN, NaN)).toBe(-110);
    });

  });

});