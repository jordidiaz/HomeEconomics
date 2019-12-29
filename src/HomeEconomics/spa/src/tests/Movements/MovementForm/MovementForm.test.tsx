import React from 'react';
import ReactDOM from 'react-dom';
import { act } from 'react-dom/test-utils';
import MovementForm from '../../../App/Movements/MovementForm/MovementForm';
import { TMovement } from '../../../App/Movements/Movement/models/movement.models';

describe('MovementForm component', () => {
  let container: Element;

  const createMovement = jest.fn((_: TMovement) => { return Promise.resolve(); });

  beforeEach(() => {
    container = document.createElement("div");
    document.body.appendChild(container);
    ReactDOM.render(<MovementForm createMovement={createMovement} />, container);
  });

  afterEach(() => {
    ReactDOM.unmountComponentAtNode(container);
    document.body.removeChild(container);
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  test('should call to createMovement when the save button is clicked', () => {
    const saveButton = container.getElementsByClassName("MovementForm__save")[0];
    act(() => {
      saveButton.dispatchEvent(new MouseEvent("click", { bubbles: true }));
    });
    expect(createMovement.mock.calls.length).toBe(1);
  });
});