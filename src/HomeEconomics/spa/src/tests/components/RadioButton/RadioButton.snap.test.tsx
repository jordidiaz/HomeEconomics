import React from "react";
import { create } from 'react-test-renderer';
import RadioButton from '../../../App/components/RadioButton/RadioButton';


describe('RadioButton component', () => {
  test('Matches the snapshot if checked', () => {
    const value = 1;
    const label = 'label';

    const radioButtonRenderer = create(<RadioButton value={value} label={label} checked={true} handleMonthChange={() => { return; }} />);
    expect(radioButtonRenderer.toJSON()).toMatchSnapshot();
  });

  test('Matches the snapshot if not checked', () => {
    const value = 1;
    const label = 'label';

    const radioButtonRenderer = create(<RadioButton value={value} label={label} checked={false} handleMonthChange={() => { return; }} />);
    expect(radioButtonRenderer.toJSON()).toMatchSnapshot();
  });
});