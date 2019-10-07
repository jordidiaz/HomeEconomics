import React from 'react';
import ReactDOM from 'react-dom';
import App from '../App/App';

describe('App component', () => {

  let container: Element;

  beforeEach(() => {
    container = document.createElement("div");
    document.body.appendChild(container);
  });

  afterEach(() => {
    ReactDOM.unmountComponentAtNode(container);
    document.body.removeChild(container);
  });

  test('should render without crashing', () => {
    ReactDOM.render(<App />, container);
    expect(container.innerHTML).toBeTruthy();
  });
});


