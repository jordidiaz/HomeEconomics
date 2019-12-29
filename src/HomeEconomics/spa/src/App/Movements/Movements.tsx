import React, { useEffect, useState } from 'react';
import Movement from './Movement/Movement';
import MovementForm from './MovementForm/MovementForm';
import { TMovement } from './Movement/models/movement.models';
import './Movements.scss';
import movementsService from './services/movements.service';

const Movements: React.FC = () => {
  const [movements, setMovements] = useState<TMovement[]>([]);

  async function deleteMovement(movement: TMovement) {
    await movementsService.remove(movement);
    setMovements(movements.filter(m => m.id !== movement.id));
  }

  async function createMovement(movement: TMovement) {
    const id = await movementsService.create(movement);
    setMovements([...movements, { ...movement, id }]);
  }

  useEffect(() => {
    (async function () {
      const movements = await movementsService.getAll();
      setMovements(movements);
    })();
  }, []);

  return (
    <div className="Movements">
      <MovementForm createMovement={createMovement} />
      <ul >
        {
          movements.map((movement: TMovement) =>
            <li key={movement.id}><Movement movement={movement} deleteMovement={deleteMovement} /></li>
          )
        }
      </ul>
    </div>
  );
};

export default Movements;