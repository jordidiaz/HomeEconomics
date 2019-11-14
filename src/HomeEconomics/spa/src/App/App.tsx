import React from 'react';
import './App.scss';
import Movements from './Movements/Movements';
import { toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

toast.configure();

const App: React.FC = () => {
  return (
    <div className="App">
      <Movements />
    </div>
  );
};

export default App;
