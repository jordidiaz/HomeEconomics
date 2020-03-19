import React from 'react';
import ReactDOM from 'react-dom';
import { act } from 'react-dom/test-utils';
import Movement from '../../../App/Movements/components/Movement/Movement';
import { getMovements } from '../../builders/movements';


describe('Movement component', () => {
  let container: Element;

  const movement = getMovements()[0];
  const deleteMovement = jest.fn();
  const loadMovement = jest.fn();

  beforeEach(async () => {
    container = document.createElement("div");
    document.body.appendChild(container);
    await act(async () => {
      ReactDOM.render(<Movement movement={movement} deleteMovement={deleteMovement} loadMovement={loadMovement} />, container);
    });
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

  test('should call to loadMovement when the edit icon is clicked', () => {
    const editIcon = container.getElementsByClassName("icon--pencil")[0];
    act(() => {
      editIcon.dispatchEvent(new MouseEvent("click", { bubbles: true }));
    });
    expect(loadMovement.mock.calls.length).toBe(1);
  });
});