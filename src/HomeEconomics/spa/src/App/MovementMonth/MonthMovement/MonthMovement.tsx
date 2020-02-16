import React from 'react';
import { MovementType } from '../../Movements/Movement/models/movement.models';
import { TMonthMovement } from '../models/movement-month.models';
import './MonthMovement.scss';

export type MonthMovementProps = {
  monthMovement: TMonthMovement;
}

const MonthMovement: React.FC<MonthMovementProps> = (props) => {

  const { monthMovement } = props;

  const type: string = monthMovement.type === MovementType.Expense
    ? 'expense'
    : 'income';

  const sign: string = monthMovement.type === MovementType.Expense
    ? '-'
    : '+';

  return (
    <div className="MonthMovement">
      <div className="MonthMovement__name">
        <span>
          {monthMovement.name}
        </span>
      </div>
      <div className="MonthMovement__amount">
        <span className={`MonthMovement__amount-${type}`}>
          {sign}{monthMovement.amount} €
        </span>
      </div>
    </div>
  );

};

export default MonthMovement;