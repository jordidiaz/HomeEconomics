import React from 'react';
import ReactDOM from 'react-dom';
import { act } from 'react-dom/test-utils';
import Movement from '../../../App/Movements/Movement/Movement';
import { getMovements } from '../../builders/movements';


describe('Movement component', () => {
  let container: Element;

  const movement = getMovements()[0];
  const deleteMovement = jest.fn();

  beforeEach(() => {
    container = document.createElement("div");
    document.body.appendChild(container);
    ReactDOM.render(<Movement movement={movement} deleteMovement={deleteMovement} />, container);
  });

  afterEach(() => {
    ReactDOM.unmountComponentAtNode(container);
    document.body.removeChild(container);
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  test('should call to deleteMovement when the delete icon is clicked', () => {
    const deleteIcon = container.getElementsByClassName("icon--bin")[0];
    act(() => {
      deleteIcon.dispatchEvent(new MouseEvent("click", { bubbles: true }));
    });
    expect(deleteMovement.mock.calls.length).toBe(1);
  });
});