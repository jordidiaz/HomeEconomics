import React from 'react';
import './Spinner.scss';
import Loader from 'react-loader-spinner';
import "react-loader-spinner/dist/loader/css/react-spinner-loader.css";

export type SpinnerProps = {
  show: boolean;
}

const Spinner: React.FC<SpinnerProps> = (props: SpinnerProps) => {

  const { show } = props;

  const klasses: string = show
    ? 'Spinner'
    : 'Spinner Spinner--hide';

  return (
    <div className={klasses}>
      <div className="Spinner__item">
        <Loader type="Puff" color="#43994d" />
      </div>
    </div>
  );
};

export default Spinner;
