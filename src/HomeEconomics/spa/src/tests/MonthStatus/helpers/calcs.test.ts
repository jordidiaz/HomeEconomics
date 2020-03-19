import { getMovementMonth } from "../../builders/movement-months";
import { calculateRemaining } from "../../../App/MovementMonth/helpers/calcs";

describe('calcs', () => {

  fdescribe('calculateRemaining', () => {

    const movementMonth = getMovementMonth();

    test('should return the remaining', () => {
      expect(calculateRemaining(movementMonth)).toBe(939.14);
    });

  });

});