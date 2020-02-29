import React, { useEffect, useState, ChangeEvent } from 'react';
import { TMonthMovement, TMovementMonth } from './models/movement-month.models';
import MonthMovement from './MonthMovement/MonthMovement';
import './MovementMonth.scss';
import movementMonthService from './services/movement-months.service';
import CheckBox from '../components/CheckBox/CheckBox';

export type MovementMonthProps = {
  initialShowPaid: boolean;
  movementMonthUpdated: (movementMonth: TMovementMonth) => void;
}

const MovementMonth: React.FC<MovementMonthProps> = (props) => {

  const { initialShowPaid, movementMonthUpdated } = props;

  const [movementMonth, setMovementMonth] = useState<TMovementMonth>();
  const [showPaid, setShowPaid] = useState<boolean>(initialShowPaid);

  const now = new Date();
  const year = now.getFullYear();
  const month = now.getMonth() + 1;

  async function createMovementMonth() {
    const movementMonth = await movementMonthService.create(year, month);
    setUpdatedMovementMonth(movementMonth);
  }

  async function payMonthMovement(monthMovement: TMonthMovement) {
    setUpdatedMovementMonth(await movementMonthService.payMonthMovement(movementMonth as TMovementMonth, monthMovement));
  }

  async function unpayMonthMovement(monthMovement: TMonthMovement) {
    setUpdatedMovementMonth(await movementMonthService.unpayMonthMovement(movementMonth as TMovementMonth, monthMovement));
  }

  async function updateMonthMovementAmount(monthMovement: TMonthMovement, newAmount: number) {
    setUpdatedMovementMonth(await movementMonthService.updateMonthMovementAmount(movementMonth as TMovementMonth, monthMovement, newAmount));
  }

  function handleChange(event: ChangeEvent<HTMLInputElement>): void {
    setShowPaid(event.target.checked);
  }

  function setUpdatedMovementMonth(movementMonth: TMovementMonth): void {
    setMovementMonth(movementMonth);
    movementMonthUpdated(movementMonth);
  }

  function filterByShowPaid(monthMovement: TMonthMovement): boolean {
    return showPaid
      ? true
      : !monthMovement.paid;
  }

  useEffect(() => {
    (async function () {
      const movementMonth = await movementMonthService.get(year, month);
      setUpdatedMovementMonth(movementMonth);
    })();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [year, month]);

  return (
    <div className="MovementMonth" >
      <CheckBox value={showPaid} label='Mostrar pagados' checked={showPaid} handleChange={handleChange} />
      <ul>
        {
          movementMonth && movementMonth.monthMovements.filter(filterByShowPaid).map((monthMovement: TMonthMovement) =>
            <li key={monthMovement.id}><MonthMovement monthMovement={monthMovement} payMonthMovement={payMonthMovement} unpayMonthMovement={unpayMonthMovement} updateMonthMovementAmount={updateMonthMovementAmount} /></li>
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