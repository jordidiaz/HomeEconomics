import { getMovementMonth } from "../../builders/movement-months";
import { calculateRemaining } from "../../../App/MonthStatus/helpers/calcs";

describe('calcs', () => {

  fdescribe('calculateRemaining', () => {

    const movementMonth = getMovementMonth();

    test('should return the remaining', () => {
      expect(calculateRemaining(movementMonth, 1333, 10)).toBe(1232.14);
      expect(calculateRemaining(movementMonth, NaN, 10)).toBe(-100.86);
      expect(calculateRemaining(movementMonth, 300, NaN)).toBe(189.14);
      expect(calculateRemaining(movementMonth, NaN, NaN)).toBe(-110.86);
    });

  });

});