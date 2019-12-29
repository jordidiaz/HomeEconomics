import React, { ChangeEvent } from 'react';
import './RadioButton.scss';

export type RadioButtonProps = {
  value: number;
  label: string;
  checked: boolean;
  handleMonthChange: (event: ChangeEvent<HTMLInputElement>) => void
}

const RadioButton: React.FC<RadioButtonProps> = (props) => {

  const { value, label, checked, handleMonthChange } = props;

  return (
    <div className="RadioButton">
      <input type="radio" id={value.toString()} value={value + 1} onChange={handleMonthChange} checked={checked} />
      <label htmlFor={value.toString()}>{label}</label>
    </div>
  );
};

export default RadioButton;
