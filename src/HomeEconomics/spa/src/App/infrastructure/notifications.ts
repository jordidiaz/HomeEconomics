import { toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

toast.configure();

const error = (message: string): void => {
  if (message) {
    toast.error(message);
  }
};

const notifications = {
  error
};

export default notifications;
