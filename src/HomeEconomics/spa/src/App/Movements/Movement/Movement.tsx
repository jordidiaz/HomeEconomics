import React from 'react';
import { getFrequency, hasFrequency } from './helpers/frequency';
import { MovementType, TMovement } from './models/movement.models';
import './Movement.scss';

export type MovementProps = {
  movement: TMovement;
}

const Movement: React.FC<MovementProps> = (props) => {

  const type: string = props.movement.type === MovementType.Expense
    ? 'expense'
    : 'income';

  const sign: string = props.movement.type === MovementType.Expense
    ? '-'
    : '+';

  const movementHasFrequency: boolean = hasFrequency(props.movement);

  const frequency: string[] = getFrequency(props.movement);

  return (
    <div className="Movement">
      <div className="Movement__name">
        <span>
          {props.movement.name}
        </span>
      </div>
      <div className="Movement__amount">
        <span className={`Movement__amount-${type}`}>
          {sign}{props.movement.amount} €
        </span>
      </div>
      {
        movementHasFrequency &&
        <div className={`Movement__frequency`}>
          <i className="icon--calendar"></i>
          <div className="Movement__frequency-text-container">
            {
              frequency.map((value: string) => <span key={value} className="Movement__frequency-text">
                {value}
              </span>)
            }
          </div>
        </div>
      }
    </div>
  );
};

export default Movement;