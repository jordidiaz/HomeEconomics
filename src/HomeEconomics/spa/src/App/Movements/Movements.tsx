import React, { useEffect, useState } from 'react';
import { TMovement, emptyMovement } from './Movement/models/movement.models';
import Movement from './Movement/Movement';
import MovementForm from './MovementForm/MovementForm';
import './Movements.scss';
import movementsService from './services/movements.service';

const Movements: React.FC = () => {
  const [movements, setMovements] = useState<TMovement[]>([]);
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

  async function deleteMovement(movement: TMovement) {
    await movementsService.remove(movement);
    setMovements(movements.filter(m => m.id !== movement.id));
  }

  async function createMovement(movement: TMovement) {
    const id = await movementsService.create(movement);
    const newMovements = [...movements, { ...movement, id }].sort(sortFn());
    setMovements(newMovements);
  }

  async function editMovement(editedMovement: TMovement) {
    await movementsService.edit(editedMovement);
    const filteredMovements = movements.filter(m => m.id !== editedMovement.id);
    filteredMovements.push(editedMovement);
    const newMovements = [...filteredMovements].sort(sortFn());
    setMovements(newMovements);
  }

  function loadMovement(movement: TMovement) {
    setMovement(movement);
  }

  useEffect(() => {
    (async function () {
      const movements = await movementsService.getAll();
      setMovements(movements);
    })();
  }, []);

  return (
    <div className="Movements">
      <MovementForm createMovement={createMovement} editMovement={editMovement} movement={movement} />
      <ul >
        {
          movements.map((movement: TMovement) =>
            <li key={movement.id}><Movement movement={movement} deleteMovement={deleteMovement} loadMovement={loadMovement} /></li>
          )
        }
      </ul>
    </div>
  );
};

export default Movements;