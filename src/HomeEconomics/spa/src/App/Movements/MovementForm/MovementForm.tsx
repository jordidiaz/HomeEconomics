import React, { ChangeEvent, useState } from 'react';
import { FrequencyType, MovementType, TMovement } from '../Movement/models/movement.models';
import './MovementForm.scss';
import { getMonthName, months } from '../Movement/helpers/months';
import RadioButton from '../../components/RadioButton/RadioButton';
import CheckBox from '../../components/CheckBox/CheckBox';

export type MovementFormProps = {
  createMovement: (movement: TMovement) => Promise<void>;
}

const MovementForm: React.FC<MovementFormProps> = (props) => {

  const { createMovement } = props;

  const emptyMovement: TMovement = {
    id: -1,
    name: '',
    amount: 0,
    type: MovementType.Undefined,
    frequencyType: FrequencyType.Undefined,
    frequencyMonth: -1,
    frequencyMonths: Array.from({ length: 12 }, () => false)
  };

  const [movement, setMovement] = useState<TMovement>(emptyMovement);

  function handleInputChange(event: ChangeEvent<HTMLInputElement>): void {
    const target = event.target;
    const value = target.value;
    const name = target.name;
    setMovement({ ...movement, [name]: value });
  }

  function handleSelectChange(event: ChangeEvent<HTMLSelectElement>): void {
    const target = event.target;
    const value = parseInt(target.value);
    const name = target.name;
    setMovement({ ...movement, [name]: value });
  }

  function handleMonthChange(event: ChangeEvent<HTMLInputElement>): void {
    const target = event.target;
    const value = parseInt(target.value);
    setMovement({ ...movement, frequencyMonth: value });
  }

  function handleMonthsChange(event: ChangeEvent<HTMLInputElement>): void {
    const target = event.target;
    const value = parseInt(target.value);
    const months = movement.frequencyMonths;
    months[value - 1] = true;
    setMovement({ ...movement, frequencyMonths: months });
  }

  function save(event: any): void {
    event.preventDefault();
    createMovement(movement).then(() => {
      setMovement(emptyMovement);
    });
  }

  return (
    <form className="MovementForm">
      <div className="MovementForm__inputs">
        <div className="MovementForm__inputs-basic">
          <input className="input form-control" value={movement.name} placeholder="Nombre" type="text" name="name" onChange={handleInputChange} />
          <input className="input form-control" value={movement.amount} placeholder="Importe" type="number" min="0" name="amount" onChange={handleInputChange} />
          <select className="select form-control" value={movement.type} name="type" onChange={handleSelectChange}>
            <option value="">Tipo</option>
            <option value="1">Gasto</option>
            <option value="0">Ingreso</option>
          </select>
          <select className="select form-control" value={movement.frequencyType} name="frequencyType" onChange={handleSelectChange}>
            <option value="">Frecuencia</option>
            <option value="0">Sin frecuencia</option>
            <option value="1">Mensual</option>
            <option value="2">Anual</option>
            <option value="3">Personalizada</option>
          </select>
        </div>
        <div className="MovementForm__inputs-months">
          {
            movement.frequencyType === FrequencyType.Yearly &&
            months.map((month: string, index: number) =>
              <RadioButton key={index} value={index} label={getMonthName(month)} checked={movement.frequencyMonth - 1 === index} handleMonthChange={handleMonthChange} />
            )
          }
          {
            movement.frequencyType === FrequencyType.Custom &&
            months.map((month: string, index: number) =>
              <CheckBox key={index} value={index} label={getMonthName(month)} checked={movement.frequencyMonths[index]} handleMonthsChange={handleMonthsChange} />
            )
          }
        </div>
      </div>
      <button className="MovementForm__save button" onClick={save}>Guardar</button>
    </form>
  );
};

export default MovementForm;