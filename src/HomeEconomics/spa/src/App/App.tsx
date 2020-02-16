import React, { useState } from 'react';
import './App.scss';
import Spinner from './components/Spinner/Spinner';
import http from './infrastructure/http';
import MovementMonth from './MovementMonth/MovementMonth';
import Movements from './Movements/Movements';

const App: React.FC = () => {
  const [loading, setLoading] = useState<boolean>(false);

  const loadingCallback = (loading: boolean): void => {
    setLoading(loading);
  };

  http.configure(loadingCallback);

  return (
    <div className="App">
      <Spinner show={loading} />
      <div className="App__movement-month">
        <MovementMonth />
      </div>
      <div className="App__movements">
        <Movements />
      </div>
    </div>
  );
};

export default App;
