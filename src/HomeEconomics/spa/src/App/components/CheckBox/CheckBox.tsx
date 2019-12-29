import React, { ChangeEvent } from 'react';
import './CheckBox.scss';

export type CheckBoxProps = {
  value: number;
  label: string;
  checked: boolean;
  handleMonthsChange: (event: ChangeEvent<HTMLInputElement>) => void
}

const CheckBox: React.FC<CheckBoxProps> = (props) => {

  const { value, label, handleMonthsChange, checked } = props;

  return (
    <div className="CheckBox">
      <input type="checkbox" id={value.toString()} value={value + 1} onChange={handleMonthsChange} checked={checked} />
      <label htmlFor={value.toString()}>{label}</label>
    </div>
  );
};

export default CheckBox;