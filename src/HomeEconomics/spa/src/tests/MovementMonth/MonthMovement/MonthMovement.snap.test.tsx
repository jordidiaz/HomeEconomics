import React from "react";
import { create } from "react-test-renderer";
import { TMonthMovement } from "../../../App/MovementMonth/models/movement-month.models";
import MonthMovement from "../../../App/MovementMonth/MonthMovement/MonthMovement";
import { MovementType } from "../../../App/Movements/Movement/models/movement.models";

describe('MonthMovement component', () => {
  test('Matches the snapshot 1', () => {
    const monthMovement: TMonthMovement = {
      id: 1,
      name: 'Name',
      type: MovementType.Expense,
      amount: 50
    };

    const monthMovementRenderer = create(<MonthMovement monthMovement={monthMovement} />);
    expect(monthMovementRenderer.toJSON()).toMatchSnapshot();
  });

  test('Matches the snapshot 2', () => {
    const monthMovement: TMonthMovement = {
      id: 1,
      name: 'Name',
      type: MovementType.Income,
      amount: 50
    };

    const monthMovementRenderer = create(<MonthMovement monthMovement={monthMovement} />);
    expect(monthMovementRenderer.toJSON()).toMatchSnapshot();
  });
});