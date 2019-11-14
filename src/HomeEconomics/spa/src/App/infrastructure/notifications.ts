import { toast } from 'react-toastify';

toast.configure();

const error = (message: string): void => {
  toast.error(message);
};

const notifications = {
  error
};

export default notifications;
