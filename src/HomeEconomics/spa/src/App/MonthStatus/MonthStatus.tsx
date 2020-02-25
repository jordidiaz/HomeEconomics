import React, { ChangeEvent, useEffect, useState } from 'react';
import { TMovementMonth } from '../MovementMonth/models/movement-month.models';
import './MonthStatus.scss';
import { calculateRemaining } from './helpers/calcs';

export type MonthStatusProps = {
  movementMonth: TMovementMonth | undefined;
}

const MonthStatus: React.FC<MonthStatusProps> = (props) => {

  const { movementMonth } = props;

  const [accountAmount, setAccountAmount] = useState<number>(0);
  const [cashAmount, setCashAmount] = useState<number>(0);
  const [remainingAmount, setRemainingAmount] = useState<number>(0);

  function handleAccountAmountChange(event: ChangeEvent<HTMLInputElement>): void {
    setAccountAmount(parseFloat(event.target.value));
  }

  function handleCashAmountChange(event: ChangeEvent<HTMLInputElement>): void {
    setCashAmount(parseFloat(event.target.value));
  }

  useEffect(() => {
    setRemainingAmount(calculateRemaining(movementMonth, accountAmount, cashAmount));
  }, [movementMonth, accountAmount, cashAmount]);

  return (
    <form className="MonthStatus">
      <div className="input-with-label">
        <label>Cuenta</label>
        <input className="input form-control" value={accountAmount} type="number" min="0" name="amount" onChange={handleAccountAmountChange} />
      </div>
      <div className="input-with-label">
        <label>Cash</label>
        <input className="input form-control" value={cashAmount} type="number" min="0" name="amount" onChange={handleCashAmountChange} />
      </div>
      <label className="MonthStatus__remaining">
        <span className={remainingAmount >= 0 ? 'MonthStatus__remaining-amount' : 'MonthStatus__remaining-amount--negative'}>{remainingAmount >= 0 ? `Sobra ${remainingAmount}` : `Falta ${remainingAmount}`}</span>
      </label>
    </form>
  );
};

export default MonthStatus;