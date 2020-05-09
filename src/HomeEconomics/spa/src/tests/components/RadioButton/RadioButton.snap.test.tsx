import React from "react";
import { create } from 'react-test-renderer';
import RadioButton from '../../../App/components/RadioButton/RadioButton';


describe('RadioButton component', () => {
  test('Matches the snapshot if checked', () => {
    const value = 1;
    const label = 'label';
    const name = 'name';

    const radioButtonRenderer = create(<RadioButton name={name} value={value} label={label} checked={true} handleMonthChange={(): void => { return; }} />);
    expect(radioButtonRenderer.toJSON()).toMatchSnapshot();
  });

  test('Matches the snapshot if not checked', () => {
    const value = 1;
    const label = 'label';

    const radioButtonRenderer = create(<RadioButton name={name} value={value} label={label} checked={false} handleMonthChange={(): void => { return; }} />);
    expect(radioButtonRenderer.toJSON()).toMatchSnapshot();
  });
});