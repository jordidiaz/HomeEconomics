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
    jest.spyOn(movementsService, 'getAll')
      .mockImplementation(() => {
        return Promise.resolve(movements);
      });

    jest.spyOn(movementsService, 'remove')
      .mockImplementation(() => {
        return Promise.resolve();
      });

    jest.spyOn(movementsService, 'create')
      .mockImplementation(() => {
        return Promise.resolve(1);
      });

    jest.spyOn(movementsService, 'edit')
      .mockImplementation(() => {
        return Promise.resolve();
      });
  });

  afterEach(() => {
    ReactDOM.unmountComponentAtNode(container);
    document.body.removeChild(container);
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  test("should call to getAll and render the MovementForm and the movements", async () => {
    await act(async () => {
      ReactDOM.render(<Movements />, container);
    });
    expect(movementsService.getAll).toHaveBeenCalledTimes(1);
    expect(container.getElementsByTagName('li').length).toBe(5);
    expect(container.getElementsByClassName('MovementForm').length).toBe(1);
  });

  test('should call to remove and remove the movement from the list', async () => {
    await act(async () => {
      ReactDOM.render(<Movements />, container);
    });
    const movement = container.getElementsByClassName('Movement')[0];
    const deleteIcon = movement.getElementsByClassName("icon--bin")[0];
    await act(async () => {
      deleteIcon.dispatchEvent(new MouseEvent("click", { bubbles: true }));
    });
    expect(movementsService.remove).toHaveBeenCalledTimes(1);
    expect(container.getElementsByTagName('li').length).toBe(4);
  });

  test('should call to create and add the movement to the list', async () => {
    await act(async () => {
      ReactDOM.render(<Movements />, container);
    });
    const saveButton = container.getElementsByClassName('MovementForm__save')[0];
    await act(async () => {
      saveButton.dispatchEvent(new MouseEvent("click", { bubbles: true }));
    });
    expect(movementsService.create).toHaveBeenCalledTimes(1);
    expect(container.getElementsByTagName('li').length).toBe(6);
  });

  test('should call to edit and edit the movement from the list', async () => {
    await act(async () => {
      ReactDOM.render(<Movements />, container);
    });
    const editButton = container.getElementsByClassName('icon--pencil')[0];
    act(() => {
      editButton.dispatchEvent(new MouseEvent("click", { bubbles: true }));
    });
    const saveButton = container.getElementsByClassName('MovementForm__save')[0];
    await act(async () => {
      saveButton.dispatchEvent(new MouseEvent("click", { bubbles: true }));
    });
    expect(movementsService.edit).toHaveBeenCalledTimes(1);
    expect(container.getElementsByTagName('li').length).toBe(5);
  });
});