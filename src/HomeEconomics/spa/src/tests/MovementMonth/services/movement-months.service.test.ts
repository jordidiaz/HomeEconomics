import http from '../../../App/infrastructure/http';
import { TMovementMonth, TMonthMovement } from '../../../App/MovementMonth/models/movement-month.models';
import movementMonthsService from '../../../App/MovementMonth/services/movement-months.service';
import { getMovementMonth } from '../../builders/movement-months';

describe('MovementMonths Service', () => {

  let movementMonth: TMovementMonth;
  let monthMovement: TMonthMovement;
  beforeEach(() => {
    movementMonth = getMovementMonth();
    monthMovement = movementMonth.monthMovements[0];
  });

  describe('get', () => {

    test('should return a movementMonth', () => {

      jest.spyOn(http, 'get')
        .mockResolvedValueOnce(movementMonth);

      expect(movementMonthsService.get(2020, 5)).resolves.toEqual(movementMonth);
    });

  });

  describe('create', () => {

    test('should create a movementMonth', () => {
      jest.spyOn(http, 'post')
        .mockResolvedValueOnce(movementMonth);

      expect(movementMonthsService.create(2020, 7)).resolves.toEqual(movementMonth);
    });
  });

  describe('payMonthMovement', () => {

    test('should pay a movementMonth', () => {
      jest.spyOn(http, 'post')
        .mockResolvedValueOnce(movementMonth);

      expect(movementMonthsService.payMonthMovement(movementMonth, monthMovement)).resolves.toEqual(movementMonth);
    });
  });

  describe('unpayMonthMovement', () => {

    test('should unpay a movementMonth', () => {
      jest.spyOn(http, 'post')
        .mockResolvedValueOnce(movementMonth);

      expect(movementMonthsService.unpayMonthMovement(movementMonth, monthMovement)).resolves.toEqual(movementMonth);
    });
  });

  describe('updateMonthMovementAmount', () => {

    test('should update movementMonths amount', () => {
      jest.spyOn(http, 'post')
        .mockResolvedValueOnce(movementMonth);

      expect(movementMonthsService.updateMonthMovementAmount(movementMonth, monthMovement, 60)).resolves.toEqual(movementMonth);
    });
  });

});