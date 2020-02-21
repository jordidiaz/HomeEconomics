import React from "react";
import ReactDOM from "react-dom";
import { act } from "react-dom/test-utils";
import { TMovementMonth } from "../../App/MovementMonth/models/movement-month.models";
import MovementMonth from "../../App/MovementMonth/MovementMonth";
import movementMonthsService from '../../App/MovementMonth/services/movement-months.service';
import { getMovementMonth } from "../builders/movement-months";

describe('MovementMonth component', () => {

  let container: Element;

  let movementMonth = getMovementMonth();

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

    jest.spyOn(movementMonthsService, 'payMonthMovement')
      .mockImplementation(() => {
        return Promise.resolve(movementMonth);
      });

    jest.spyOn(movementMonthsService, 'unpayMonthMovement')
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
      ReactDOM.render(<MovementMonth initialShowPaid={true} />, container);
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
      ReactDOM.render(<MovementMonth initialShowPaid={true} />, container);
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
      ReactDOM.render(<MovementMonth initialShowPaid={true} />, container);
    });
    const deleteIcon = container.getElementsByClassName("MovementMonth__create")[0];
    await act(async () => {
      deleteIcon.dispatchEvent(new MouseEvent("click", { bubbles: true }));
    });
    expect(movementMonthsService.create).toHaveBeenCalledTimes(1);
  });

  test('should call to payMonthMovement when pay is clicked', async () => {
    await act(async () => {
      ReactDOM.render(<MovementMonth initialShowPaid={true} />, container);
    });
    const monthMovement = container.getElementsByClassName('MonthMovement')[0];
    const payCheckbox = monthMovement.getElementsByClassName("icon--checkbox-unchecked")[0];
    await act(async () => {
      payCheckbox.dispatchEvent(new MouseEvent("click", { bubbles: true }));
    });
    expect(movementMonthsService.payMonthMovement).toHaveBeenCalledTimes(1);
  });

  test('should call to unpayMonthMovement when unpay is clicked', async () => {
    await act(async () => {
      ReactDOM.render(<MovementMonth initialShowPaid={true} />, container);
    });

    const monthMovement = container.getElementsByClassName('MonthMovement')[1];
    const payCheckbox = monthMovement.getElementsByClassName("icon--checkbox-checked")[0];
    await act(async () => {
      payCheckbox.dispatchEvent(new MouseEvent("click", { bubbles: true }));
    });
    expect(movementMonthsService.unpayMonthMovement).toHaveBeenCalledTimes(1);
  });

});