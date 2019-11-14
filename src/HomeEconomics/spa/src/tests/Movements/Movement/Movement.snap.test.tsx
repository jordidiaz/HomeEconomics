import React from "react";
import { create } from "react-test-renderer";
import Movement from '../../../App/Movements/Movement/Movement';
import { FrequencyType, MovementType, TMovement } from '../../../App/Movements/Movement/models/movement.models';

describe("Movement component", () => {
  test("Matches the snapshot", () => {
    const movement: TMovement = {
      id: 1,
      name: 'Basura',
      amount: 40,
      type: MovementType.Expense,
      frequencyType: FrequencyType.Custom,
      frequencyMonths: [
        false,
        true,
        false,
        false,
        false,
        true,
        false,
        false,
        false,
        true,
        false,
        false
      ]
    };

    const movementRenderer = create(<Movement movement={movement} deleteMovement={() => { return; }} />);
    expect(movementRenderer.toJSON()).toMatchSnapshot();
  });
});