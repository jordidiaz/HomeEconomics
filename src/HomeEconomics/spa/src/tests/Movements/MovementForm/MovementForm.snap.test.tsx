import React from "react";
import { create } from 'react-test-renderer';
import { TMovement, emptyMovement } from '../../../App/Movements/Movement/models/movement.models';
import MovementForm from '../../../App/Movements/MovementForm/MovementForm';

describe('MovementForm component', () => {
  test('Matches the snapshot', () => {
    const movementFormRenderer = create(<MovementForm movement={emptyMovement} createMovement={(_: TMovement) => { return Promise.resolve(); }} editMovement={(_: TMovement) => { return Promise.resolve(); }} />);
    expect(movementFormRenderer.toJSON()).toMatchSnapshot();
  });

});