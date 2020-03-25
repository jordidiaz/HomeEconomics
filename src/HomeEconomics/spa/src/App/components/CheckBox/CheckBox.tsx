import React, { ChangeEvent } from 'react';
import './CheckBox.scss';

export type CheckBoxProps = {
  value: number | boolean;
  label: string;
  checked: boolean;
  handleChange: (event: ChangeEvent<HTMLInputElement>) => void;
}

const CheckBox: React.FC<CheckBoxProps> = (props: CheckBoxProps) => {

  const { value, label, handleChange, checked } = props;

  return (
    <div className="CheckBox">
      <input type="checkbox" id={value.toString()} value={value.toString()} onChange={handleChange} checked={checked} />
      <label htmlFor={value.toString()}>{label}</label>
    </div>
  );
};

export default CheckBox;