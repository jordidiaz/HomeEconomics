import React from "react";
import ReactDOM from "react-dom";
import { act } from "react-dom/test-utils";
import { TMovementMonth } from "../../App/MovementMonth/models/movement-month.models";
import MovementMonth from "../../App/MovementMonth/MovementMonth";
import movementMonthsService from '../../App/MovementMonth/services/movement-months.service';
import { getMovementMonth } from "../builders/movement-months";

describe('MovementMonth component', () => {

  let container: Element;

  const movementMonth = getMovementMonth();

  beforeEach(() => {
    container = document.createElement("div");
    document.body.appendChild(container);
  });

  beforeEach(() => {
    jest.spyOn(movementMonthsService, 'get')
      .mockImplementation(() => {
        return Promise.resolve(movementMonth);
      });

    jest.spyOn(movementMonthsService, 'create')
      .mockImplementation(() => {
        return Promise.resolve(movementMonth);
      });
  });

  afterEach(() => {
    ReactDOM.unmountComponentAtNode(container);
    document.body.removeChild(container);
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  test('should call to get and render the MonthMovements if MovementMonth exists', async () => {
    await act(async () => {
      ReactDOM.render(<MovementMonth />, container);
    });
    expect(movementMonthsService.get).toHaveBeenCalledTimes(1);
    expect(container.getElementsByTagName('li').length).toBe(2);
    expect(container.getElementsByClassName('MovementMonth__create').length).toBe(0);
  });

  test('should call to get and render the MonthMovement create button if MovementMonth not exists', async () => {
    jest.spyOn(movementMonthsService, 'get')
      .mockImplementation(() => {
        return Promise.resolve(null as unknown as TMovementMonth);
      });

    await act(async () => {
      ReactDOM.render(<MovementMonth />, container);
    });
    expect(movementMonthsService.get).toHaveBeenCalledTimes(1);
    expect(container.getElementsByTagName('li').length).toBe(0);
    expect(container.getElementsByClassName('MovementMonth__create').length).toBe(1);
  });

  test('should call to create when create button is clicked', async () => {
    jest.spyOn(movementMonthsService, 'get')
      .mockImplementation(() => {
        return Promise.resolve(null as unknown as TMovementMonth);
      });

    await act(async () => {
      ReactDOM.render(<MovementMonth />, container);
    });
    const deleteIcon = container.getElementsByClassName("MovementMonth__create")[0];
    await act(async () => {
      deleteIcon.dispatchEvent(new MouseEvent("click", { bubbles: true }));
    });
    expect(movementMonthsService.create).toHaveBeenCalledTimes(1);
  });

});