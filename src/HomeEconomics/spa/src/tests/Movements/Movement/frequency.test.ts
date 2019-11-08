import { hasFrequency, getFrequency } from '../../../App/Movements/Movement/helpers/frequency';
import { FrequencyType, MovementType, TMovement } from '../../../App/Movements/Movement/models/movement.models';


describe('frequency', () => {

  let movement: TMovement;
  beforeEach(() => {
    movement = {
      id: 1,
      name: 'name',
      amount: 10,
      type: MovementType.Expense,
      frequencyType: FrequencyType.None,
      frequencyMonths: []
    };
  });

  describe('hasFrequency', () => {

    test('should return false if frequencyType is None', () => {
      expect(hasFrequency(movement)).toBeFalsy();
    });

    test('should return true if frequencyType is Monthly', () => {
      movement.frequencyType = FrequencyType.Monthly;
      expect(hasFrequency(movement)).toBeTruthy();
    });

    test('should return true if frequencyType is Yearly', () => {
      movement.frequencyType = FrequencyType.Yearly;
      expect(hasFrequency(movement)).toBeTruthy();
    });

    test('should return true if frequencyType is Custom', () => {
      movement.frequencyType = FrequencyType.Custom;
      expect(hasFrequency(movement)).toBeTruthy();
    });

  });

  describe('getFrequency', () => {

    test('should return [] if frequencyType is None', () => {
      expect(getFrequency(movement)).toEqual([]);
    });

    test('should return [] if frequencyType is not None and frequencyMonths.length > 12', () => {
      movement.frequencyType = FrequencyType.Custom;
      movement.frequencyMonths = [
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false
      ];
      expect(getFrequency(movement)).toEqual([]);
    });

    test('should return ["men"] if frequencyType is Monthly', () => {
      movement.frequencyType = FrequencyType.Monthly;
      expect(getFrequency(movement)).toEqual(['men']);
    });

    test('should return ["ago"] if frequencyType is Yearly and august selected', () => {
      movement.frequencyType = FrequencyType.Yearly;
      movement.frequencyMonths = [
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        true,
        false,
        false,
        false,
        false
      ];
      expect(getFrequency(movement)).toEqual(['ago']);
    });

    test('should return ["ene", "feb", "nov"] if frequencyType is Custom and those months are selected', () => {
      movement.frequencyType = FrequencyType.Yearly;
      movement.frequencyMonths = [
        true,
        true,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        true,
        false
      ];
      expect(getFrequency(movement)).toEqual(['ene', 'feb', 'nov']);
    });

  });
});