import React, { ChangeEvent, useState } from "react";
import { MovementType } from "../../Movements/Movement/models/movement.models";
import './AddMonthMovementForm.scss';

export type AddMonthMovementProps = {
  addMonthMovement: (name: string, amount: number, movementType: MovementType) => Promise<void>;
}

const AddMonthMovementForm: React.FC<AddMonthMovementProps> = (props) => {

  const { addMonthMovement } = props;

  const [name, setName] = useState<string>();
  const [amount, setAmount] = useState<number>(0);
  const [type, setType] = useState<MovementType>(MovementType.Undefined);

  function save(event: any): void {
    event.preventDefault();
    addMonthMovement(name as string, amount, type)
      .then(() => cleanForm());
  };

  function cancel(event: any): void {
    event.preventDefault();
    cleanForm();
  };

  function cleanForm(): void {
    setName('');
    setAmount(0);
    setType(MovementType.Undefined);
  }

  function handleNameChange(event: ChangeEvent<HTMLInputElement>): void {
    setName(event.target.value);
  }

  function handleAmountChange(event: ChangeEvent<HTMLInputElement>): void {
    setAmount(parseFloat(event.target.value));
  }

  function handleTypeChange(event: ChangeEvent<HTMLSelectElement>): void {
    setType(isNaN(parseInt(event.target.value)) ? MovementType.Undefined : parseInt(event.target.value));
  }

  return (
    <form className="AddMonthMovementForm">
      <div className="AddMonthMovementForm__inputs-basic">
        <input className="input form-control" value={name} placeholder="Nombre" type="text" name="name" onChange={handleNameChange} />
        <input className="input form-control" value={amount} placeholder="Importe" type="number" min="0" name="amount" onChange={handleAmountChange} />
        <select className="select form-control" value={type} name="type" onChange={handleTypeChange}>
          <option value="">Tipo</option>
          <option value="1">Gasto</option>
          <option value="0">Ingreso</option>
        </select>
      </div>
      <button className="AddMonthMovementForm__save button" onClick={save}>Guardar</button>
      <button className="AddMonthMovementForm__cancel button" onClick={cancel}>Cancelar</button>
    </form>
  );

};

export default AddMonthMovementForm;