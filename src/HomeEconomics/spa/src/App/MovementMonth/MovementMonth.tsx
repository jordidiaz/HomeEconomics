import React, { ChangeEvent, useState } from 'react';
import CheckBox from '../components/CheckBox/CheckBox';
import useForm from '../hooks/useForm';
import { MovementType } from '../Movements/models/movement.models';
import AddMonthMovementForm from './components/AddMonthMovementForm/AddMonthMovementForm';
import MonthMovement from './components/MonthMovement/MonthMovement';
import MonthStatus from './components/MonthStatus/MonthStatus';
import { useMovementMonth } from './hooks/useMovementMonth';
import { TMonthMovement, TMovementMonth } from './models/movement-month.models';
import './MovementMonth.scss';
import movementMonthService from './services/movement-months.service';
import { parseNumber } from '../helpers/number-parser';

export type MovementMonthProps = {
  initialShowPaid: boolean;
}

type MonthSelectorValues = {
  year: string;
  month: string;
}

const MovementMonth: React.FC<MovementMonthProps> = (props: MovementMonthProps) => {

  const now = new Date();
  const currentYear = now.getFullYear();
  const currentMonth = now.getMonth() + 1;

  const { initialShowPaid } = props;

  const initialValues: MonthSelectorValues = {
    year: currentYear.toString(),
    month: currentMonth.toString()
  }

  // eslint-disable-next-line @typescript-eslint/no-use-before-define
  const { handleChange, values } = useForm<MonthSelectorValues>(initialValues);

  const [showPaid, setShowPaid] = useState<boolean>(initialShowPaid);
  const { movementMonth, setMovementMonth } = useMovementMonth(parseNumber(values.year), parseNumber(values.month));

  async function createMovementMonth(): Promise<void> {
    const movementMonth = await movementMonthService.create(parseNumber(values.year), parseNumber(values.month));
    setMovementMonth(movementMonth);
  }

  async function payMonthMovement(monthMovement: TMonthMovement): Promise<void> {
    setMovementMonth(await movementMonthService.payMonthMovement(movementMonth as TMovementMonth, monthMovement));
  }

  async function unpayMonthMovement(monthMovement: TMonthMovement): Promise<void> {
    setMovementMonth(await movementMonthService.unpayMonthMovement(movementMonth as TMovementMonth, monthMovement));
  }

  async function updateMonthMovementAmount(monthMovement: TMonthMovement, newAmount: number): Promise<void> {
    setMovementMonth(await movementMonthService.updateMonthMovementAmount(movementMonth as TMovementMonth, monthMovement, newAmount));
  }

  async function addMonthMovement(name: string, amount: number, movementType: MovementType): Promise<void> {
    setMovementMonth(await movementMonthService.addMonthMovement(movementMonth as TMovementMonth, name, amount, movementType));
  }

  async function deleteMonthMovement(monthMovement: TMonthMovement): Promise<void> {
    setMovementMonth(await movementMonthService.deleteMonthMovement(movementMonth as TMovementMonth, monthMovement));
  }

  async function addStatus(movementMonth: TMovementMonth, accountAmount: number, cashAmount: number): Promise<void> {
    setMovementMonth(await movementMonthService.addStatus(movementMonth, accountAmount, cashAmount));
  }

  function handleShowPaidChange(event: ChangeEvent<HTMLInputElement>): void {
    setShowPaid(event.target.checked);
  }

  function filterByShowPaid(monthMovement: TMonthMovement): boolean {
    return showPaid
      ? true
      : !monthMovement.paid;
  }

  return (
    <div className="MovementMonth" >
      <div className="MovementMonth__month-status">
        {
          movementMonth &&
          <MonthStatus movementMonth={movementMonth} addStatus={addStatus} />
        }
      </div>
      <div className="MovementMonth__tools">
        <div className="MovementMonth__tools-add-month-movement">
          {
            movementMonth &&
            <AddMonthMovementForm addMonthMovement={addMonthMovement} />
          }
        </div>
        <div className="MovementMonth__tools-month-selector">
          <form noValidate>
            <select className="select form-control" value={values.year} name="year" onChange={handleChange}>
              <option value="">Año</option>
              <option value="2020">2020</option>
              <option value="2021">2021</option>
            </select>
            <select className="select form-control" value={values.month} name="month" onChange={handleChange}>
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
          </form>
        </div>
      </div>
      {
        movementMonth &&
        <CheckBox name="showPaid" value={showPaid} label='Mostrar pagados' checked={showPaid} handleChange={handleShowPaidChange} />
      }
      <ul>
        {
          movementMonth?.monthMovements.filter(filterByShowPaid).map((monthMovement: TMonthMovement) =>
            <li key={monthMovement.id}><MonthMovement monthMovement={monthMovement} payMonthMovement={payMonthMovement} unpayMonthMovement={unpayMonthMovement} updateMonthMovementAmount={updateMonthMovementAmount} deleteMonthMovement={deleteMonthMovement} /></li>
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