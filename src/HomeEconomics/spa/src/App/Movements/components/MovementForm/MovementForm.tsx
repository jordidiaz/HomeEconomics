import React, { ChangeEvent } from 'react';
import CheckBox from '../../../components/CheckBox/CheckBox';
import RadioButton from '../../../components/RadioButton/RadioButton';
import useForm from '../../../hooks/useForm';
import { getMonthName, months } from '../../helpers/months';
import { useMovement } from '../../hooks/useMovement';
import { createEmpyMovement, emptyMovement, FrequencyType, TMovement, MovementType } from '../../models/movement.models';
import './MovementForm.scss';
import { parseNumber } from '../../../helpers/number-parser';

export type MovementFormProps = {
  movement: TMovement;
  createMovement: (movement: TMovement) => Promise<void>;
  editMovement: (movement: TMovement) => Promise<void>;
}

export type MovementFormValues = {
  id: number;
  name: string;
  amount: string;
  type: MovementType;
  frequencyType: FrequencyType;
  frequencyMonth: string;
  frequencyMonths: boolean[];
}

const MovementForm: React.FC<MovementFormProps> = (props: MovementFormProps) => {

  const { createMovement, editMovement, movement } = props;

  const initialValues: MovementFormValues = {
    id: movement.id,
    name: movement.name,
    amount: movement.amount.toString(),
    type: movement.type,
    frequencyType: movement.frequencyType,
    frequencyMonth: movement.frequencyMonth.toString(),
    frequencyMonths: movement.frequencyMonths
  }

  function createTMovementFromValues(values: MovementFormValues): TMovement {
    const movement: TMovement = {
      id: values.id,
      name: values.name,
      amount: parseNumber(values.amount),
      type: values.type,
      frequencyType: values.frequencyType,
      frequencyMonth: parseNumber(values.frequencyMonth),
      frequencyMonths: values.frequencyMonths
    }

    return movement;
  }

  function createValuesFromTMovement(movement: TMovement): MovementFormValues {
    const values: MovementFormValues = {
      id: movement.id,
      name: movement.name,
      amount: movement.amount.toString(),
      type: movement.type,
      frequencyType: movement.frequencyType,
      frequencyMonth: movement.frequencyMonth.toString(),
      frequencyMonths: movement.frequencyMonths
    }

    return values;
  }

  // eslint-disable-next-line @typescript-eslint/no-use-before-define
  const { handleChange, handleSubmit, values, setValues } = useForm<MovementFormValues>(initialValues, submit);
  useMovement(movement, setValues);

  function handleMonthsChange(event: ChangeEvent<HTMLInputElement>): void {
    const target = event.target;
    const value = parseInt(target.value);
    const months = values.frequencyMonths;
    months[value - 1] = target.checked;
    setValues({ ...values, frequencyMonths: months });
  }

  function createOrEditMovement(): Promise<void> {
    return values.id === emptyMovement.id ? createMovement(createTMovementFromValues(values)) : editMovement(createTMovementFromValues(values));
  }

  function submit(): void {
    createOrEditMovement().then(() => {
      setValues(createValuesFromTMovement(createEmpyMovement()));
    });
  }

  function cancel(event: React.MouseEvent<HTMLButtonElement, MouseEvent>): void {
    event.preventDefault();
    setValues(createValuesFromTMovement(createEmpyMovement()));
  }

  return (
    <form noValidate onSubmit={handleSubmit} className="MovementForm">
      <div className="MovementForm__inputs">
        <div className="MovementForm__inputs-basic">
          <input className="input form-control" value={values.name} placeholder="Nombre" type="text" name="name" onChange={handleChange} />
          <input className="input form-control" value={values.amount} placeholder="Importe" type="number" min="0" name="amount" onChange={handleChange} />
          <select className="select form-control" value={values.type} name="type" onChange={handleChange}>
            <option value="">Tipo</option>
            <option value="1">Gasto</option>
            <option value="0">Ingreso</option>
          </select>
          <select className="select form-control" value={values.frequencyType} name="frequencyType" onChange={handleChange}>
            <option value="">Frecuencia</option>
            <option value="0">Sin frecuencia</option>
            <option value="1">Mensual</option>
            <option value="2">Anual</option>
            <option value="3">Personalizada</option>
          </select>
        </div>
        <div className="MovementForm__inputs-months">
          {
            values.frequencyType === FrequencyType.Yearly &&
            months.map((month: string, index: number) =>
              <RadioButton name="frequencyMonth" key={index} value={index} label={getMonthName(month)} checked={parseNumber(values.frequencyMonth) - 1 === index} handleMonthChange={handleChange} />
            )
          }
          {
            values.frequencyType === FrequencyType.Custom &&
            months.map((month: string, index: number) =>
              <CheckBox name="frequencyMonths" key={index} value={index + 1} label={getMonthName(month)} checked={values.frequencyMonths[index]} handleChange={handleMonthsChange} />
            )
          }
        </div>
      </div>
      <button type="submit" className="MovementForm__save button">Guardar</button>
      <button className="MovementForm__cancel button" onClick={cancel}>Cancelar</button>
    </form>
  );
};

export default MovementForm;