import http from '../../../App/infrastructure/http';
import { TMovementMonth } from '../../../App/MovementMonth/models/movement-month.models';
import movementMonthsService from '../../../App/MovementMonth/services/movement-months.service';
import { getMovementMonth } from '../../builders/movement-months';

describe('MovementMonths Service', () => {

  let movementMonth: TMovementMonth;
  beforeEach(() => {
    movementMonth = getMovementMonth();
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

});