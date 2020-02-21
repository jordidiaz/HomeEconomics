import React, { useEffect, useState, ChangeEvent } from 'react';
import { TMonthMovement, TMovementMonth } from './models/movement-month.models';
import MonthMovement from './MonthMovement/MonthMovement';
import './MovementMonth.scss';
import movementMonthService from './services/movement-months.service';
import CheckBox from '../components/CheckBox/CheckBox';

export type MovementMonthProps = {
  initialShowPaid: boolean;
}

const MovementMonth: React.FC<MovementMonthProps> = ({ initialShowPaid = true }) => {

  // const { initialShowPaid } = props;

  const [movementMonth, setMovementMonth] = useState<TMovementMonth>();
  const [showPaid, setShowPaid] = useState<boolean>(initialShowPaid);

  const now = new Date();
  const year = now.getFullYear();
  const month = now.getMonth() + 1;

  async function createMovementMonth() {
    const movementMonth = await movementMonthService.create(year, month);
    setMovementMonth(movementMonth);
  }

  async function payMonthMovement(monthMovement: TMonthMovement) {
    setMovementMonth(await movementMonthService.payMonthMovement(movementMonth as TMovementMonth, monthMovement));
  }

  async function unpayMonthMovement(monthMovement: TMonthMovement) {
    setMovementMonth(await movementMonthService.unpayMonthMovement(movementMonth as TMovementMonth, monthMovement));
  }

  function handleChange(event: ChangeEvent<HTMLInputElement>): void {
    setShowPaid(event.target.checked);
  }

  function filterByShowPaid(monthMovement: TMonthMovement): boolean {
    return showPaid
      ? true
      : !monthMovement.paid;
  }

  useEffect(() => {
    (async function () {
      const movementMonth = await movementMonthService.get(year, month);
      setMovementMonth(movementMonth);
    })();
  }, [year, month]);

  return (
    <div className="MovementMonth" >
      <CheckBox value={showPaid} label='Mostrar pagados' checked={showPaid} handleChange={handleChange} />
      <ul>
        {
          movementMonth && movementMonth.monthMovements.filter(filterByShowPaid).map((monthMovement: TMonthMovement) =>
            <li key={monthMovement.id}><MonthMovement monthMovement={monthMovement} payMonthMovement={payMonthMovement} unpayMonthMovement={unpayMonthMovement} /></li>
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