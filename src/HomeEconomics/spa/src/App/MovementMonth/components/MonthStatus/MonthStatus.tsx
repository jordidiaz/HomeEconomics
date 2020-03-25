import React, { ChangeEvent, useEffect, useState } from 'react';
import { calculateRemaining } from '../../helpers/calcs';
import { TMovementMonth } from '../../models/movement-month.models';
import './MonthStatus.scss';

export type MonthStatusProps = {
  movementMonth: TMovementMonth | undefined;
  addStatus: (movementMonth: TMovementMonth, accountAmount: number, cashAmount: number) => Promise<void>;
}

const MonthStatus: React.FC<MonthStatusProps> = (props: MonthStatusProps) => {
  const { movementMonth, addStatus } = props;

  const [remainingAmount, setRemainingAmount] = useState<number>(0);
  const [accountAmount, setAccountAmount] = useState<string>('0');
  const [cashAmount, setCashAmount] = useState<string>('0');

  function handleAmountBlur(): void {
    if (movementMonth) {
      addStatus(movementMonth, parseFloat(accountAmount), parseFloat(cashAmount));
    }
  }

  function handleAccountAmountChange(event: ChangeEvent<HTMLInputElement>): void {
    event.target.value
      ? setAccountAmount(event.target.value)
      : setAccountAmount('');
  }

  function handleCashAmountChange(event: ChangeEvent<HTMLInputElement>): void {
    event.target.value
      ? setCashAmount(event.target.value)
      : setCashAmount('');
  }

  useEffect(() => {
    setRemainingAmount(calculateRemaining(movementMonth));
    setAccountAmount(movementMonth ? movementMonth.status.accountAmount.toString() : '0');
    setCashAmount(movementMonth ? movementMonth.status.cashAmount.toString() : '0');
  }, [movementMonth]);

  return (
    <form className="MonthStatus">
      <div className="input-with-label">
        <label>Cuenta</label>
        <input className="input form-control" value={accountAmount} type="number" min="0" name="amount" onBlur={handleAmountBlur} onChange={handleAccountAmountChange} />
      </div>
      <div className="input-with-label">
        <label>Cash</label>
        <input className="input form-control" value={cashAmount} type="number" min="0" name="amount" onBlur={handleAmountBlur} onChange={handleCashAmountChange} />
      </div>
      <label className="MonthStatus__remaining">
        <span className={remainingAmount >= 0 ? 'MonthStatus__remaining-amount' : 'MonthStatus__remaining-amount--negative'}>{remainingAmount >= 0 ? `Sobra ${remainingAmount}` : `Falta ${remainingAmount}`}</span>
      </label>
    </form>
  );
};

export default MonthStatus;