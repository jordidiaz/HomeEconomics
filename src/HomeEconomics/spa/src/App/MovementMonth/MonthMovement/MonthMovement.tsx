import React from 'react';
import { MovementType } from '../../Movements/Movement/models/movement.models';
import { TMonthMovement } from '../models/movement-month.models';
import './MonthMovement.scss';

export type MonthMovementProps = {
  monthMovement: TMonthMovement;
  payMonthMovement: (monthMovement: TMonthMovement) => void;
  unpayMonthMovement: (monthMovement: TMonthMovement) => void;
}

const MonthMovement: React.FC<MonthMovementProps> = (props) => {

  const { monthMovement, payMonthMovement, unpayMonthMovement } = props;

  const pay = () => {
    payMonthMovement(monthMovement);
  };

  const unpay = () => {
    unpayMonthMovement(monthMovement);
  };

  const type: string = monthMovement.type === MovementType.Expense
    ? 'expense'
    : 'income';

  const sign: string = monthMovement.type === MovementType.Expense
    ? '-'
    : '+';

  return (
    <div className="MonthMovement">
      <div className="MonthMovement__name">
        {monthMovement.name}
      </div>
      <div className="MonthMovement__amount">
        <span className={`MonthMovement__amount-${type}`}>
          {sign}{monthMovement.amount} €
        </span>
      </div>
      {
        monthMovement.paid &&
        <div className="MonthMovement__paid">
          PAGADO
        </div>
      }
      <div className="MonthMovement__actions">
        {
          !monthMovement.paid &&
          <i className="action-icon icon--checkbox-unchecked" onClick={pay}></i>
        }
        {
          monthMovement.paid &&
          <i className="action-icon icon--checkbox-checked" onClick={unpay}></i>
        }
      </div>
    </div>
  );

};

export default MonthMovement;