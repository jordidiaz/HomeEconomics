import React, { useEffect, useState } from 'react';
import Movement from './Movement/Movement';
import { TMovement } from './Movement/models/movement.models';
import './Movements.scss';
import movementsService from './services/movements.service';

const Movements: React.FC = () => {
  const [movements, setMovements] = useState<TMovement[]>([]);

  useEffect(() => {
    movementsService.getAllMovements()
      .then(setMovements);
  }, []);

  return (
    <ul className="Movements">
      {
        movements.map((movement: TMovement) => {
          return <li key={movement.id}><Movement movement={movement} /></li>;
        })
      }
    </ul>
  );
};

export default Movements;