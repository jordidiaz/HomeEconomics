import React, { useState } from 'react';
import Movement from './components/Movement/Movement';
import MovementForm from './components/MovementForm/MovementForm';
import { useMovements } from './hooks/useMovements';
import { emptyMovement, TMovement } from './models/movement.models';
import './Movements.scss';
import movementsService from './services/movements.service';

export type MovementsProps = {
  addMovementToCurrentMonth: (movement: TMovement) => void;
}

const Movements: React.FC<MovementsProps> = (props: MovementsProps) => {

  const { addMovementToCurrentMonth } = props;

  const { movements, setMovements } = useMovements();
  const [movement, setMovement] = useState<TMovement>(emptyMovement);

  const sortFn = () => (a: TMovement, b: TMovement): number => {
    if (a.name < b.name) {
      return -1;
    }
    if (a.name > b.name) {
      return 1;
    }
    return 0;
  };

  async function deleteMovement(movement: TMovement): Promise<void> {
    await movementsService.remove(movement);
    setMovements(movements.filter(m => m.id !== movement.id));
  }

  async function createMovement(movement: TMovement): Promise<void> {
    const id = await movementsService.create(movement);
    const newMovements = [...movements, { ...movement, id }].sort(sortFn());
    setMovements(newMovements);
  }

  async function editMovement(editedMovement: TMovement): Promise<void> {
    await movementsService.edit(editedMovement);
    const filteredMovements = movements.filter(m => m.id !== editedMovement.id);
    filteredMovements.push(editedMovement);
    const newMovements = [...filteredMovements].sort(sortFn());
    setMovements(newMovements);
  }

  function loadMovement(movement: TMovement): void {
    setMovement(movement);
  }

  function addMovementToMonth(movement: TMovement): void {
    addMovementToCurrentMonth(movement);
  }

  return (
    <div className="Movements">
      <MovementForm createMovement={createMovement} editMovement={editMovement} movement={movement} />
      <ul>
        {
          movements.map((movement: TMovement) =>
            <li key={movement.id}><Movement movement={movement} deleteMovement={deleteMovement} loadMovement={loadMovement} addMovementToCurrentMonth={addMovementToMonth} /></li>
          )
        }
      </ul>
    </div>
  );
};

export default Movements;