import React, { useState, ChangeEvent } from 'react';
import { MovementType } from '../../../Movements/models/movement.models';
import { TMonthMovement } from '../../models/movement-month.models';
import './MonthMovement.scss';

export type MonthMovementProps = {
  monthMovement: TMonthMovement;
  payMonthMovement: (monthMovement: TMonthMovement) => void;
  unpayMonthMovement: (monthMovement: TMonthMovement) => void;
  updateMonthMovementAmount: (monthMovement: TMonthMovement, newAmount: number) => void;
}

const MonthMovement: React.FC<MonthMovementProps> = (props: MonthMovementProps) => {

  const { monthMovement, payMonthMovement, unpayMonthMovement, updateMonthMovementAmount } = props;

  const [editingAmount, setEditingAmount] = useState<boolean>(false);
  const [newAmount, setNewAmount] = useState<number>(monthMovement.amount);

  const pay = (): void => {
    payMonthMovement(monthMovement);
  };

  const unpay = (): void => {
    unpayMonthMovement(monthMovement);
  };

  const edit = (): void => {
    setEditingAmount(true);
  };

  const cancel = (): void => {
    setEditingAmount(false);
    setNewAmount(monthMovement.amount);
  };

  const accept = (): void => {
    updateMonthMovementAmount(monthMovement, newAmount);
    setEditingAmount(false);
  };

  function handleAmountChange(event: ChangeEvent<HTMLInputElement>): void {
    setNewAmount(parseFloat(event.target.value));
  }

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
        {
          !editingAmount &&
          <span className={`MonthMovement__amount-${type}`}>
            {sign}{monthMovement.amount} €
          </span>
        }
        {
          editingAmount &&
          <input className="input form-control" value={newAmount} placeholder="Importe" type="number" min="0" onChange={handleAmountChange} />
        }
      </div>
      {
        monthMovement.paid &&
        <div className="MonthMovement__paid">
          PAGADO
        </div>
      }
      <div className="MonthMovement__actions">
        {
          (!monthMovement.paid && !editingAmount) &&
          <>
            <i className="action-icon icon--pencil" onClick={edit}></i>
            <i className="action-icon icon--checkbox-unchecked" onClick={pay}></i>
          </>
        }
        {
          (!monthMovement.paid && editingAmount) &&
          <>
            <i className="action-icon icon--cross" onClick={cancel}></i>
            <i className="action-icon icon--checkmark" onClick={accept}></i>
          </>
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