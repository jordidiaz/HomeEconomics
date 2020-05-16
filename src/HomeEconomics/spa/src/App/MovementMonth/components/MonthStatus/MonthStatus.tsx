import React from 'react';
import useForm from '../../../hooks/useForm';
import { isNumber } from '../../helpers/calcs';
import { useAmounts } from '../../hooks/useAmounts';
import { TMovementMonth } from '../../models/movement-month.models';
import './MonthStatus.scss';
import { parseNumber } from '../../../helpers/number-parser';

export type MonthStatusProps = {
  movementMonth: TMovementMonth;
  addStatus: (movementMonth: TMovementMonth, accountAmount: number, cashAmount: number) => Promise<void>;
}

export type MonthStatusFormValues = {
  accountAmount: string;
  cashAmount: string;
}

const MonthStatus: React.FC<MonthStatusProps> = (props: MonthStatusProps) => {

  const { addStatus, movementMonth } = props;

  const initialValues: MonthStatusFormValues = {
    accountAmount: movementMonth.status.accountAmount.toString(),
    cashAmount: movementMonth.status.cashAmount.toString()
  }

  // eslint-disable-next-line @typescript-eslint/no-use-before-define
  const { handleChange, values, setValues } = useForm<MonthStatusFormValues>(initialValues, onBlur);
  const remainingAmount = useAmounts(movementMonth, setValues);

  function onBlur(): void {
    if (!isNumber(parseNumber(values.accountAmount)) || !isNumber(parseNumber(values.cashAmount))) {
      return;
    }
    addStatus(movementMonth, parseNumber(values.accountAmount), parseNumber(values.cashAmount));
  }

  return (
    <form noValidate onBlurCapture={onBlur} className="MonthStatus">
      <div className="input-with-label">
        <label>Cuenta</label>
        <input className="input form-control" value={values.accountAmount} type="number" min="0" name="accountAmount" onChange={handleChange} />
      </div>
      <div className="input-with-label">
        <label>Cash</label>
        <input className="input form-control" value={values.cashAmount} type="number" min="0" name="cashAmount" onChange={handleChange} />
      </div>
      <label className="MonthStatus__remaining">
        <span className={remainingAmount >= 0 ? 'MonthStatus__remaining-amount' : 'MonthStatus__remaining-amount--negative'}>{remainingAmount >= 0 ? `Sobra ${remainingAmount}` : `Falta ${remainingAmount}`}</span>
      </label>
    </form>
  );
};

export default MonthStatus;