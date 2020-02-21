import React, { ChangeEvent } from 'react';
import './CheckBox.scss';

export type CheckBoxProps = {
  value: any;
  label: string;
  checked: boolean;
  handleChange: (event: ChangeEvent<HTMLInputElement>) => void
}

const CheckBox: React.FC<CheckBoxProps> = (props) => {

  const { value, label, handleChange, checked } = props;

  return (
    <div className="CheckBox">
      <input type="checkbox" id={value.toString()} value={value + 1} onChange={handleChange} checked={checked} />
      <label htmlFor={value.toString()}>{label}</label>
    </div>
  );
};

export default CheckBox;