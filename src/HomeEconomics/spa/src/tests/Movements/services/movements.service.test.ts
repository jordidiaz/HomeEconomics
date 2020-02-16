import http from '../../../App/infrastructure/http';
import { TMovement } from '../../../App/Movements/Movement/models/movement.models';
import movementsService from '../../../App/Movements/services/movements.service';
import { getMovements } from '../../builders/movements';

describe('Movements Service', () => {

  let movements: TMovement[];

  describe('getAll', () => {

    movements = getMovements(3);
    const expected: TMovement[] = getMovements(3);

    test('should return all movements', () => {
      jest.spyOn(http, 'get')
        .mockResolvedValueOnce({ movements });

      expect(movementsService.getAll()).resolves.toEqual(expected);
    });

  });

  describe('remove', () => {

    test('should remove the movement', () => {
      jest.spyOn(http, 'del')
        .mockResolvedValue(true);

      expect(movementsService.remove(movements[0])).resolves.not.toThrow();
    });

  });

  describe('create', () => {

    test('should create a movement', () => {
      jest.spyOn(http, 'post')
        .mockResolvedValueOnce(1);

      const movement = getMovements()[0];

      expect(movementsService.create(movement)).resolves.toEqual(1);
    });
  });

  describe('edit', () => {

    test('should edit a movement', () => {
      jest.spyOn(http, 'put')
        .mockResolvedValueOnce(true);

      const movement = getMovements()[0];

      expect(movementsService.edit(movement)).resolves.not.toThrow();
    });
  });

});