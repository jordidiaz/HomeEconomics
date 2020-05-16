import React from "react";
import useForm from "../../../hooks/useForm";
import { MovementType } from "../../../Movements/models/movement.models";
import './AddMonthMovementForm.scss';
import { parseNumber } from "../../../helpers/number-parser";

export type AddMonthMovementProps = {
  addMonthMovement: (name: string, amount: number, movementType: MovementType) => Promise<void>;
}

type AddMonthMovementFormValues = {
  name: string;
  amount: string;
  type: MovementType;
}

const AddMonthMovementForm: React.FC<AddMonthMovementProps> = (props: AddMonthMovementProps) => {

  const { addMonthMovement } = props;

  const initialValues: AddMonthMovementFormValues = {
    name: '',
    amount: '0',
    type: MovementType.Undefined
  }

  // eslint-disable-next-line @typescript-eslint/no-use-before-define
  const { handleChange, handleSubmit, values, setValues } = useForm<AddMonthMovementFormValues>(initialValues, submit);

  function cleanForm(): void {
    setValues(initialValues);
  }

  function submit(): void {
    addMonthMovement(values.name, parseNumber(values.amount), values.type)
      .then(() => cleanForm());
  };

  function cancel(event: React.MouseEvent<HTMLButtonElement, MouseEvent>): void {
    event.preventDefault();
    cleanForm();
  };

  return (
    <form noValidate onSubmit={handleSubmit} className="AddMonthMovementForm">
      <div className="AddMonthMovementForm__inputs-basic">
        <input className="input form-control" value={values.name} placeholder="Nombre" type="text" name="name" onChange={handleChange} />
        <input className="input form-control" value={values.amount} placeholder="Importe" type="number" min="0" name="amount" onChange={handleChange} />
        <select className="select form-control" value={values.type} name="type" onChange={handleChange}>
          <option value="">Tipo</option>
          <option value="1">Gasto</option>
          <option value="0">Ingreso</option>
        </select>
      </div>
      <button type="submit" className="AddMonthMovementForm__save button">Guardar</button>
      <button className="AddMonthMovementForm__cancel button" onClick={cancel}>Cancelar</button>
    </form>
  );

};

export default AddMonthMovementForm;