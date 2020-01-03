import axios, { AxiosError, AxiosInstance, AxiosResponse, AxiosRequestConfig } from 'axios';
import notifications from './notifications';

let axiosInstance: AxiosInstance;

const configure = (loadingCallback: (loading: boolean) => void): AxiosInstance => {
  axiosInstance = axios.create({
    baseURL: `${process.env.REACT_APP_API_BASE_URL}/api/`
  });

  const onRejected: (error: AxiosError) => void = (error: AxiosError) => {
    loadingCallback(false);
    notifications.error(getErrorMessage(error));
    return Promise.reject(error);
  };

  axiosInstance.interceptors.request.use(
    ((request: AxiosRequestConfig) => {
      loadingCallback(true);
      return request;
    }),
    onRejected
  );

  axiosInstance.interceptors.response.use(
    ((response: AxiosResponse<any>) => {
      loadingCallback(false);
      return response;
    }),
    onRejected
  );

  return axiosInstance;
};

const getErrorMessage = (error: AxiosError): string => {
  const defaultMessage = 'Ha ocurrido un error inesperado';
  const validationErrorMessage = 'Ha ocurrido un error de validación';

  if (error.response && error.response.status === 500) {
    return defaultMessage;
  }

  if (error.response && error.response.status === 400) {
    return validationErrorMessage;
  }

  if (error && !error.response) {
    return error.message;
  }

  if (error && error.response) {
    return error.response.data.detail;
  }

  return defaultMessage;
};

const get = async <T>(path: string): Promise<T> => {
  const response = await axiosInstance.get(path);
  return response.data;
};

const del = async (path: string): Promise<boolean> => {
  await axiosInstance.delete(path);
  return true;
};

const post = async (path: string, data: any): Promise<any> => {
  const response = await axiosInstance.post(path, data);
  return response.data;
};

const put = async (path: string, data: any): Promise<boolean> => {
  await axiosInstance.put(path, data);
  return true;
};

export default {
  configure,
  get,
  del,
  post,
  put
};