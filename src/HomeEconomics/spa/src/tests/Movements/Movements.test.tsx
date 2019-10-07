import React from 'react';
import ReactDOM from "react-dom";
import { act } from "react-dom/test-utils";
import Movements from '../../App/Movements/Movements';
import movementsService from '../../App/Movements/services/movements.service';
import { getMovements } from '../builders/movements';

describe("Movements component", () => {

  let container: Element;

  const movements = getMovements(5);

  beforeEach(() => {
    container = document.createElement("div");
    document.body.appendChild(container);
  });

  beforeEach(() => {
    jest.spyOn(movementsService, 'getAllMovements')
      .mockImplementation(() => {
        return Promise.resolve(movements);
      });
  });

  afterEach(() => {
    ReactDOM.unmountComponentAtNode(container);
    document.body.removeChild(container);
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  test("should call to getAllMovements and render the movements", async () => {
    await act(async () => {
      ReactDOM.render(<Movements />, container);
    });
    expect(movementsService.getAllMovements).toHaveBeenCalledTimes(1);
    expect(container.getElementsByTagName('li').length).toBe(5);
  });
});