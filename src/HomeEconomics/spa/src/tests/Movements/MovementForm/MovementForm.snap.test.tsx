import React from "react";
import { create } from 'react-test-renderer';
import MovementForm from '../../../App/Movements/components/MovementForm/MovementForm';
import { emptyMovement } from '../../../App/Movements/models/movement.models';

describe('MovementForm component', () => {
  test('Matches the snapshot', () => {
    const movementFormRenderer = create(<MovementForm movement={emptyMovement} createMovement={(): Promise<void> => { return Promise.resolve(); }} editMovement={(): Promise<void> => { return Promise.resolve(); }} />);
    expect(movementFormRenderer.toJSON()).toMatchSnapshot();
  });

});