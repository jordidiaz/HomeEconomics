import { parseNumber } from "../../App/helpers/number-parser";

describe('number parser', () => {

  describe('parseNumber', () => {

    test('should parse the number', () => {
      expect(parseNumber('15')).toBe(15);
      expect(parseNumber('15.')).toBe(15.00);
      expect(parseNumber('15.0')).toBe(15.00);
      expect(parseNumber('15.00')).toBe(15.00);
      expect(parseNumber('15.1')).toBe(15.10);
      expect(parseNumber('15.11')).toBe(15.11);
      expect(parseNumber('15.111')).toBe(15.11);
      expect(parseNumber('15.99')).toBe(15.99);
      expect(parseNumber('15.999')).toBe(15.99);
    });

  });

});