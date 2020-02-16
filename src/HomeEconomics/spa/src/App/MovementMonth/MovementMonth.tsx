import React, { useEffect, useState } from 'react';
import { TMonthMovement, TMovementMonth } from './models/movement-month.models';
import MonthMovement from './MonthMovement/MonthMovement';
import './MovementMonth.scss';
import movementMonthService from './services/movement-months.service';

const MovementMonth: React.FC = () => {
  const [movementMonth, setMovementMonth] = useState<TMovementMonth>();

  const now = new Date();
  const year = now.getFullYear();
  const month = now.getMonth();

  async function createMovementMonth() {
    const movementMonth = await movementMonthService.create(year, month);
    setMovementMonth(movementMonth);
  }

  useEffect(() => {
    (async function () {
      const movementMonth = await movementMonthService.get(year, month);
      setMovementMonth(movementMonth);
    })();
  }, [year, month]);

  return (
    <div className="MovementMonth" >
      <ul>
        {
          movementMonth && movementMonth.monthMovements.map((monthMovement: TMonthMovement) =>
            <li key={monthMovement.id}><MonthMovement monthMovement={monthMovement} /></li>
          )
        }
      </ul>
      {
        !movementMonth &&
        <div className="MovementMonth__actions">
          <button className="MovementMonth__create button" onClick={createMovementMonth}>Crear</button>
        </div>
      }
    </div>
  );
};

export default MovementMonth;