import React, { useState } from 'react';
import './App.scss';
import Spinner from './components/Spinner/Spinner';
import http from './infrastructure/http';
import MovementMonth from './MovementMonth/MovementMonth';
import { emptyMovement, TMovement } from './Movements/models/movement.models';
import Movements from './Movements/Movements';

const App: React.FC = () => {
  const [loading, setLoading] = useState<boolean>(false);
  const [movementToAdd, setMovementToAdd] = useState<TMovement>(emptyMovement);

  const loadingCallback = (loading: boolean): void => {
    setLoading(loading);
  };

  http.configure(loadingCallback);

  const addMovementToCurrentMonth = (movement: TMovement): void => {
    setMovementToAdd(movement);
  }

  const addMovementSuccess = (): void => {
    setMovementToAdd(emptyMovement);
  }

  return (
    <div className="App">
      <Spinner show={loading} />
      <div className="App__movement-month">
        <MovementMonth initialShowPaid={false} movementToAdd={movementToAdd} addMovementSuccess={addMovementSuccess} />
      </div>
      <div className="App__movements">
        <Movements addMovementToCurrentMonth={addMovementToCurrentMonth} />
      </div>
    </div>
  );
};

export default App;
