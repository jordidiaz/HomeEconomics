import { TMovement } from '../../../App/Movements/Movement/models/movement.models';
import movementsService from '../../../App/Movements/services/movements.service';
import { getMovements } from '../../builders/movements';

describe('Movements Service', () => {

  describe('getAllMovements', () => {

    let response: any;
    const movements: TMovement[] = getMovements(3);
    const expected: TMovement[] = getMovements(3);

    beforeEach(() => {
      response = {
        json: () => Promise.resolve({
          movements: movements
        })
      }

      jest.spyOn(window, 'fetch')
        .mockImplementation(() => {
          return Promise.resolve(response);
        });
    });

    test('should return all movements', () => {
      return expect(movementsService.getAllMovements()).resolves.toEqual(expected);
    });

  });

});