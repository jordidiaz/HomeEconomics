import http from '../../../App/infrastructure/http';
import { TMovement } from '../../../App/Movements/Movement/models/movement.models';
import movementsService from '../../../App/Movements/services/movements.service';
import { getMovements } from '../../builders/movements';

describe('Movements Service', () => {

  let movements: TMovement[];

  describe('getAllMovements', () => {

    movements = getMovements(3);
    const expected: TMovement[] = getMovements(3);

    test('should return all movements', () => {
      jest.spyOn(http, 'get')
        .mockResolvedValueOnce({ movements });

      return expect(movementsService.getAllMovements()).resolves.toEqual(expected);
    });

  });

  describe('deleteMovement', () => {

    test('should delete the movement', () => {
      jest.spyOn(http, 'del')
        .mockResolvedValueOnce(true);

      return expect(movementsService.deleteMovement(movements[0])).resolves.not.toThrow();
    });

  });

});