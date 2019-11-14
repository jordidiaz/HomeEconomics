import React, { useEffect, useState } from 'react';
import Movement from './Movement/Movement';
import { TMovement } from './Movement/models/movement.models';
import './Movements.scss';
import movementsService from './services/movements.service';

const Movements: React.FC = () => {
  const [movements, setMovements] = useState<TMovement[]>([]);

  async function deleteMovement(movement: TMovement) {
    await movementsService.deleteMovement(movement);
    setMovements(movements.filter(m => m.id !== movement.id));
  }

  useEffect(() => {
    (async function () {
      const movements = await movementsService.getAllMovements();
      setMovements(movements);
    })();
  }, []);

  return (
    <ul className="Movements">
      {
        movements.map((movement: TMovement) =>
          <li key={movement.id}><Movement movement={movement} deleteMovement={deleteMovement} /></li>
        )
      }
    </ul>
  );
};

export default Movements;