import React from 'react';
import ReactDOM from "react-dom";
import { act } from "react-dom/test-utils";
import MonthMovement from "../../../App/MovementMonth/MonthMovement/MonthMovement";
import { getMovementMonth } from "../../builders/movement-months";


describe('MonthMovement component', () => {
  let container: Element;

  const monthMovement = getMovementMonth().monthMovements[0];
  const pay = jest.fn();
  const unpay = jest.fn();
  const accept = jest.fn();

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

  test('should call to payMonthMovement when the pay is checked', () => {
    monthMovement.paid = false;
    act(() => {
      ReactDOM.render(<MonthMovement monthMovement={monthMovement} payMonthMovement={pay} unpayMonthMovement={unpay} updateMonthMovementAmount={accept} />, container);
    });
    const payCheckbox = container.getElementsByClassName("icon--checkbox-unchecked")[0];
    act(() => {
      payCheckbox.dispatchEvent(new MouseEvent("click", { bubbles: true }));
    });
    expect(pay.mock.calls.length).toBe(1);
  });

  test('should call to unpayMonthMovement when the pay is unchecked', () => {
    monthMovement.paid = true;
    act(() => {
      ReactDOM.render(<MonthMovement monthMovement={monthMovement} payMonthMovement={pay} unpayMonthMovement={unpay} updateMonthMovementAmount={accept} />, container);
    });
    const payCheckbox = container.getElementsByClassName("icon--checkbox-checked")[0];
    act(() => {
      payCheckbox.dispatchEvent(new MouseEvent("click", { bubbles: true }));
    });
    expect(unpay.mock.calls.length).toBe(1);
  });

  test('should call to updateMonthMovementAmount when accept is clicked', () => {
    monthMovement.paid = false;
    act(() => {
      ReactDOM.render(<MonthMovement monthMovement={monthMovement} payMonthMovement={pay} unpayMonthMovement={unpay} updateMonthMovementAmount={accept} />, container);
    });
    const edit = container.getElementsByClassName("icon--pencil")[0];
    act(() => {
      edit.dispatchEvent(new MouseEvent("click", { bubbles: true }));
    });
    const acceptIcon = container.getElementsByClassName("icon--checkmark")[0];
    act(() => {
      acceptIcon.dispatchEvent(new MouseEvent("click", { bubbles: true }));
    });
    expect(accept.mock.calls.length).toBe(1);
  });

});