import React from 'react';
import ReactDOM from 'react-dom';
import { act } from 'react-dom/test-utils';
import MovementForm from '../../../App/Movements/components/MovementForm/MovementForm';
import { TMovement, emptyMovement } from '../../../App/Movements/models/movement.models';
import { getMovements } from '../../builders/movements';

describe('MovementForm component', () => {
  let container: Element;

  const createMovement = jest.fn((_: TMovement) => { return Promise.resolve(); });
  const editMovement = jest.fn((_: TMovement) => { return Promise.resolve(); });

  beforeEach(() => {
    container = document.createElement("div");
    document.body.appendChild(container);
  });

  afterEach(() => {
    ReactDOM.unmountComponentAtNode(container);
    document.body.removeChild(container);
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  test('should call to createMovement when the save button is clicked', () => {
    act(() => {
      ReactDOM.render(<MovementForm movement={emptyMovement} createMovement={createMovement} editMovement={editMovement} />, container);
    });
    const saveButton = container.getElementsByClassName("MovementForm__save")[0];
    act(() => {
      saveButton.dispatchEvent(new MouseEvent("click", { bubbles: true }));
    });
    expect(createMovement.mock.calls.length).toBe(1);
  });

  test('should call to editMovement when the save button is clicked', () => {
    const movement = getMovements()[0];
    act(() => {
      ReactDOM.render(<MovementForm movement={movement} createMovement={createMovement} editMovement={editMovement} />, container);
    });
    const saveButton = container.getElementsByClassName("MovementForm__save")[0];
    act(() => {
      saveButton.dispatchEvent(new MouseEvent("click", { bubbles: true }));
    });
    expect(editMovement.mock.calls.length).toBe(1);
  });
});