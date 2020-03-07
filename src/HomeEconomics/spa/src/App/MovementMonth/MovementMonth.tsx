import React, { useEffect, useState, ChangeEvent } from 'react';
import { TMonthMovement, TMovementMonth } from './models/movement-month.models';
import MonthMovement from './MonthMovement/MonthMovement';
import './MovementMonth.scss';
import movementMonthService from './services/movement-months.service';
import CheckBox from '../components/CheckBox/CheckBox';
import AddMonthMovementForm from './AddMonthMovementForm/AddMonthMovementForm';
import { MovementType } from '../Movements/Movement/models/movement.models';

export type MovementMonthProps = {
  initialShowPaid: boolean;
  movementMonthUpdated: (movementMonth: TMovementMonth) => void;
}

const MovementMonth: React.FC<MovementMonthProps> = (props) => {

  const now = new Date();
  const currentYear = now.getFullYear();
  const currentMonth = now.getMonth() + 1;

  const { initialShowPaid, movementMonthUpdated } = props;

  const [movementMonth, setMovementMonth] = useState<TMovementMonth>();
  const [showPaid, setShowPaid] = useState<boolean>(initialShowPaid);
  const [year, setYear] = useState<number>(currentYear);
  const [month, setMonth] = useState<number>(currentMonth);



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

  async function addMonthMovement(name: string, amount: number, movementType: MovementType) {
    setUpdatedMovementMonth(await movementMonthService.addMonthMovement(movementMonth as TMovementMonth, name, amount, movementType));
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

  function handleYearChange(event: ChangeEvent<HTMLSelectElement>): void {
    setYear(parseInt(event.target.value));
  }

  function handleMonthChange(event: ChangeEvent<HTMLSelectElement>): void {
    setMonth(parseInt(event.target.value));
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
      <>
        <div className="MovementMonth__tools">
          <div className="MovementMonth__tools-add-month-movement">
            {
              movementMonth &&
              <AddMonthMovementForm addMonthMovement={addMonthMovement} />
            }
          </div>
          <div className="MovementMonth__tools-month-selector">
            <select className="select form-control" value={year} name="type" onChange={handleYearChange}>
              <option value="">Año</option>
              <option value="2020">2020</option>
              <option value="2021">2021</option>
            </select>
            <select className="select form-control" value={month} name="type" onChange={handleMonthChange}>
              <option value="">Mes</option>
              <option value="1">Enero</option>
              <option value="2">Febrero</option>
              <option value="3">Marzo</option>
              <option value="4">Abril</option>
              <option value="5">Mayo</option>
              <option value="6">Junio</option>
              <option value="7">Julio</option>
              <option value="8">Agosto</option>
              <option value="9">Septiembre</option>
              <option value="10">Octubre</option>
              <option value="11">Noviembre</option>
              <option value="12">Diciembre</option>
            </select>
          </div>
        </div>
        {
          movementMonth &&
          <CheckBox value={showPaid} label='Mostrar pagados' checked={showPaid} handleChange={handleChange} />
        }
        <ul>
          {
            movementMonth && movementMonth.monthMovements.filter(filterByShowPaid).map((monthMovement: TMonthMovement) =>
              <li key={monthMovement.id}><MonthMovement monthMovement={monthMovement} payMonthMovement={payMonthMovement} unpayMonthMovement={unpayMonthMovement} updateMonthMovementAmount={updateMonthMovementAmount} /></li>
            )
          }
        </ul>
      </>
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