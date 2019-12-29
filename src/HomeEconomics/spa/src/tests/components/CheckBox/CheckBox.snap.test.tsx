import React from "react";
import { create } from 'react-test-renderer';
import CheckBox from '../../../App/components/CheckBox/CheckBox';


describe('CheckBox component', () => {
  test('Matches the snapshot if checked', () => {
    const value = 1;
    const label = 'label';

    const checkBoxRenderer = create(<CheckBox value={value} label={label} checked={true} handleMonthsChange={() => { return; }} />);
    expect(checkBoxRenderer.toJSON()).toMatchSnapshot();
  });

  test('Matches the snapshot if not checked', () => {
    const value = 1;
    const label = 'label';

    const checkBoxRenderer = create(<CheckBox value={value} label={label} checked={false} handleMonthsChange={() => { return; }} />);
    expect(checkBoxRenderer.toJSON()).toMatchSnapshot();
  });
});