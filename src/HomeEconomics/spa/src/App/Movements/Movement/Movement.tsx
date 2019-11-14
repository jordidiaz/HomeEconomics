import React from 'react';
import { getFrequency, hasFrequency } from './helpers/frequency';
import { MovementType, TMovement } from './models/movement.models';
import './Movement.scss';

export type MovementProps = {
  movement: TMovement;
  deleteMovement: (movement: TMovement) => void;
}

const Movement: React.FC<MovementProps> = (props) => {

  const { movement, deleteMovement } = props;

  const onClickDelete = () => {
    deleteMovement(movement);
  };

  const type: string = movement.type === MovementType.Expense
    ? 'expense'
    : 'income';

  const sign: string = movement.type === MovementType.Expense
    ? '-'
    : '+';

  const movementHasFrequency: boolean = hasFrequency(movement);

  const frequency: string[] = getFrequency(movement);

  return (
    <div className="Movement">
      <div className="Movement__name">
        <span>
          {movement.name}
        </span>
      </div>
      <div className="Movement__amount">
        <span className={`Movement__amount-${type}`}>
          {sign}{movement.amount} €
        </span>
      </div>
      {
        movementHasFrequency &&
        <div className={`Movement__frequency`}>
          <i className="icon--calendar"></i>
          <div className="Movement__frequency-text-container">
            {
              frequency.map((value: string) =>
                <span key={value} className="Movement__frequency-text">
                  {value}
                </span>)
            }
          </div>
        </div>
      }
      <div className="Movement__actions">
        <i className="action-icon icon--bin" onClick={onClickDelete}></i>
      </div>
    </div>
  );
};

export default Movement;