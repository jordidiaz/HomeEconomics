import React from "react";
import { create } from "react-test-renderer";
import Movement from '../../../App/Movements/Movement/Movement';
import { FrequencyType, MovementType, TMovement } from '../../../App/Movements/Movement/models/movement.models';

describe("Movement component", () => {
  test("Matches the snapshot 1", () => {
    const movement: TMovement = {
      id: 1,
      name: 'Name',
      amount: 40,
      type: MovementType.Expense,
      frequencyType: FrequencyType.None,
      frequencyMonth: 0,
      frequencyMonths: [
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false
      ]
    };

    const movementRenderer = create(<Movement movement={movement} deleteMovement={() => { return; }} loadMovement={() => { return; }} />);
    expect(movementRenderer.toJSON()).toMatchSnapshot();
  });

  test("Matches the snapshot 2", () => {
    const movement: TMovement = {
      id: 1,
      name: 'Name',
      amount: 40,
      type: MovementType.Income,
      frequencyType: FrequencyType.None,
      frequencyMonth: 0,
      frequencyMonths: [
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false
      ]
    };

    const movementRenderer = create(<Movement movement={movement} deleteMovement={() => { return; }} loadMovement={() => { return; }} />);
    expect(movementRenderer.toJSON()).toMatchSnapshot();
  });

  test("Matches the snapshot 3", () => {
    const movement: TMovement = {
      id: 1,
      name: 'Name',
      amount: 40,
      type: MovementType.Income,
      frequencyType: FrequencyType.Monthly,
      frequencyMonth: 0,
      frequencyMonths: [
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false
      ]
    };

    const movementRenderer = create(<Movement movement={movement} deleteMovement={() => { return; }} loadMovement={() => { return; }} />);
    expect(movementRenderer.toJSON()).toMatchSnapshot();
  });

  test("Matches the snapshot 4", () => {
    const movement: TMovement = {
      id: 1,
      name: 'Name',
      amount: 40,
      type: MovementType.Income,
      frequencyType: FrequencyType.Yearly,
      frequencyMonth: 2,
      frequencyMonths: [
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false
      ]
    };

    const movementRenderer = create(<Movement movement={movement} deleteMovement={() => { return; }} loadMovement={() => { return; }} />);
    expect(movementRenderer.toJSON()).toMatchSnapshot();
  });

  test("Matches the snapshot 2", () => {
    const movement: TMovement = {
      id: 1,
      name: 'Name',
      amount: 40,
      type: MovementType.Income,
      frequencyType: FrequencyType.Custom,
      frequencyMonth: 0,
      frequencyMonths: [
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        true,
        false,
        false,
        true,
        false
      ]
    };

    const movementRenderer = create(<Movement movement={movement} deleteMovement={() => { return; }} loadMovement={() => { return; }} />);
    expect(movementRenderer.toJSON()).toMatchSnapshot();
  });

});