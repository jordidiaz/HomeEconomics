import React from "react";
import { create } from 'react-test-renderer';
import Spinner from '../../../App/components/Spinner/Spinner';

describe('Spinner component', () => {
  test('Matches the snapshot if visible', () => {
    const spinnerRenderer = create(<Spinner show={true} />);
    expect(spinnerRenderer.toJSON()).toMatchSnapshot();
  });

  test('Matches the snapshot if not visible', () => {
    const spinnerRenderer = create(<Spinner show={false} />);
    expect(spinnerRenderer.toJSON()).toMatchSnapshot();
  });
});