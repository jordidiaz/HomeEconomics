import axios, { AxiosError, AxiosInstance, AxiosResponse } from 'axios';
import notifications from './notifications';

const configureAxios = (): AxiosInstance => {
  const axiosInstance: AxiosInstance = axios.create({
    baseURL: `${process.env.REACT_APP_API_BASE_URL}/api/`
  });

  axiosInstance.interceptors.response.use(
    ((response: AxiosResponse<any>) => response),
    ((error: AxiosError) => {
      notifications.error(getErrorMessage(error));
      return Promise.reject(error);
    })
  );

  return axiosInstance;
};

const getErrorMessage = (error: AxiosError): string => {
  if (error && !error.response) {
    return error.message;
  }

  if (error && error.response) {
    return error.response.data.detail;
  }

  return "Ha ocurrido un error inesperado";
};

const axiosInstance = configureAxios();

const get = async <T>(path: string): Promise<T> => {
  const response = await axiosInstance.get(path);
  return response.data;
};

const del = async (path: string): Promise<boolean> => {
  await axiosInstance.delete(path);
  return true;
};

export default {
  get,
  del
};