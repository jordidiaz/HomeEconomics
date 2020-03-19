import React from "react";
import { create } from "react-test-renderer";
import MonthStatus from "../../../App/MovementMonth/components/MonthStatus/MonthStatus";
import { getMovementMonth } from "../../builders/movement-months";

describe('MonthStatus component', () => {

  const movementMonth = getMovementMonth();

  test('Matches the snapshot', () => {
    const monthStatusRenderer = create(<MonthStatus status={movementMonth.status} />);
    expect(monthStatusRenderer.toJSON()).toMatchSnapshot();
  });

});