import React, { ChangeEvent, useState, useEffect } from 'react';
import CheckBox from '../../../components/CheckBox/CheckBox';
import RadioButton from '../../../components/RadioButton/RadioButton';
import { getMonthName, months } from '../../helpers/months';
import { emptyMovement, FrequencyType, TMovement, createEmpyMovement } from '../../models/movement.models';
import './MovementForm.scss';

export type MovementFormProps = {
  movement: TMovement;
  createMovement: (movement: TMovement) => Promise<void>;
  editMovement: (movement: TMovement) => Promise<void>;
}

const MovementForm: React.FC<MovementFormProps> = (props) => {
  let { movement } = props;
  const { createMovement, editMovement } = props;
  const [currentMovement, setCurrentMovement] = useState<TMovement>(movement);

  useEffect(() => {
    if (movement.id !== currentMovement.id) {
      setCurrentMovement(movement);
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [movement]);

  function handleInputChange(event: ChangeEvent<HTMLInputElement>): void {
    const target = event.target;
    const value = target.value;
    const name = target.name;
    setCurrentMovement({ ...currentMovement, [name]: value });
  }

  function handleSelectChange(event: ChangeEvent<HTMLSelectElement>): void {
    const target = event.target;
    const value = parseInt(target.value);
    const name = target.name;
    setCurrentMovement({ ...currentMovement, [name]: value });
  }

  function handleMonthChange(event: ChangeEvent<HTMLInputElement>): void {
    const target = event.target;
    const value = parseInt(target.value);
    setCurrentMovement({ ...currentMovement, frequencyMonth: value });
  }

  function handleChange(event: ChangeEvent<HTMLInputElement>): void {
    const target = event.target;
    const value = parseInt(target.value);
    const months = currentMovement.frequencyMonths;
    months[value - 1] = target.checked;
    setCurrentMovement({ ...currentMovement, frequencyMonths: months });
  }

  function save(event: any): void {
    event.preventDefault();
    createOrEditMovement().then(() => {
      setCurrentMovement(createEmpyMovement());
    });
  }

  function cancel(event: any): void {
    event.preventDefault();
    setCurrentMovement(createEmpyMovement());
  }

  function createOrEditMovement(): Promise<void> {
    return currentMovement.id === emptyMovement.id ? createMovement(currentMovement) : editMovement(currentMovement);
  }

  return (
    <>
      <form className="MovementForm">
        <div className="MovementForm__inputs">
          <div className="MovementForm__inputs-basic">
            <input className="input form-control" value={currentMovement.name} placeholder="Nombre" type="text" name="name" onChange={handleInputChange} />
            <input className="input form-control" value={currentMovement.amount} placeholder="Importe" type="number" min="0" name="amount" onChange={handleInputChange} />
            <select className="select form-control" value={currentMovement.type} name="type" onChange={handleSelectChange}>
              <option value="">Tipo</option>
              <option value="1">Gasto</option>
              <option value="0">Ingreso</option>
            </select>
            <select className="select form-control" value={currentMovement.frequencyType} name="frequencyType" onChange={handleSelectChange}>
              <option value="">Frecuencia</option>
              <option value="0">Sin frecuencia</option>
              <option value="1">Mensual</option>
              <option value="2">Anual</option>
              <option value="3">Personalizada</option>
            </select>
          </div>
          <div className="MovementForm__inputs-months">
            {
              currentMovement.frequencyType === FrequencyType.Yearly &&
              months.map((month: string, index: number) =>
                <RadioButton key={index} value={index} label={getMonthName(month)} checked={currentMovement.frequencyMonth - 1 === index} handleMonthChange={handleMonthChange} />
              )
            }
            {
              currentMovement.frequencyType === FrequencyType.Custom &&
              months.map((month: string, index: number) =>
                <CheckBox key={index} value={index} label={getMonthName(month)} checked={currentMovement.frequencyMonths[index]} handleChange={handleChange} />
              )
            }
          </div>
        </div>
      </form>
      <button className="MovementForm__save button" onClick={save}>Guardar</button>
      <button className="MovementForm__cancel button" onClick={cancel}>Cancelar</button>
    </>
  );
};

export default MovementForm;