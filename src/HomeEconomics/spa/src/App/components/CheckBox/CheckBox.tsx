import React, { ChangeEvent } from 'react';
import './CheckBox.scss';

export type CheckBoxProps = {
  name: string;
  value: number | boolean;
  label: string;
  checked: boolean;
  handleChange: (event: ChangeEvent<HTMLInputElement>) => void;
}

const CheckBox: React.FC<CheckBoxProps> = (props: CheckBoxProps) => {

  const { name, value, label, handleChange, checked } = props;

  return (
    <div className="CheckBox">
      <input type="checkbox" name={name} id={value.toString()} value={value.toString()} onChange={handleChange} checked={checked} />
      <label htmlFor={value.toString()}>{label}</label>
    </div>
  );
};

export default CheckBox;