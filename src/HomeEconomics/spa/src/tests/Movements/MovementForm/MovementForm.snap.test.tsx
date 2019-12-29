import React from "react";
import { create } from 'react-test-renderer';
import { TMovement } from '../../../App/Movements/Movement/models/movement.models';
import MovementForm from '../../../App/Movements/MovementForm/MovementForm';

describe('MovementForm component', () => {
  test('Matches the snapshot', () => {
    const movementFormRenderer = create(<MovementForm createMovement={(_: TMovement) => { return Promise.resolve(); }} />);
    expect(movementFormRenderer.toJSON()).toMatchSnapshot();
  });

});