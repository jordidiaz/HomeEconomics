import React, { useState } from 'react';
import './App.scss';
import Spinner from './components/Spinner/Spinner';
import http from './infrastructure/http';
import MovementMonth from './MovementMonth/MovementMonth';
import Movements from './Movements/Movements';
import MonthStatus from './MonthStatus/MonthStatus';
import { TMovementMonth } from './MovementMonth/models/movement-month.models';

const App: React.FC = () => {
  const [loading, setLoading] = useState<boolean>(false);
  const [movementMonth, setMovementMonth] = useState<TMovementMonth>();

  const loadingCallback = (loading: boolean): void => {
    setLoading(loading);
  };

  http.configure(loadingCallback);

  function movementMonthUpdated(movementMonth: TMovementMonth): void {
    setMovementMonth(movementMonth);
  }

  return (
    <div className="App">
      <Spinner show={loading} />
      <div className="App__month-status">
        <MonthStatus movementMonth={movementMonth} />
      </div>
      <div className="App__movement-month">
        <MovementMonth initialShowPaid={true} movementMonthUpdated={movementMonthUpdated} />
      </div>
      <div className="App__movements">
        <Movements />
      </div>
    </div>
  );
};

export default App;
