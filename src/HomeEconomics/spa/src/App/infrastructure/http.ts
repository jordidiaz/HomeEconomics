import axios, { AxiosError, AxiosInstance, AxiosResponse, AxiosRequestConfig } from 'axios';
import notifications from './notifications';

let axiosInstance: AxiosInstance;

const getErrorMessage = (error: AxiosError): string => {
  const defaultMessage = 'Ha ocurrido un error inesperado';
  const validationErrorMessage = 'Ha ocurrido un error de validación';

  if (error.response?.status === 500) {
    return defaultMessage;
  }

  if (error.response?.status === 400) {
    return validationErrorMessage;
  }

  if (error.response?.status === 404) {
    return null as unknown as string;
  }

  if (error && !error.response) {
    return error.message;
  }

  if (error?.response) {
    return error.response.data.detail;
  }

  return defaultMessage;
};

const configure = (loadingCallback: (loading: boolean) => void): AxiosInstance => {
  axiosInstance = axios.create({
    baseURL: `${process.env.REACT_APP_API_BASE_URL}/api/`
  });

  const onRejected: (error: AxiosError) => void = (error: AxiosError) => {
    loadingCallback(false);
    if (error.response?.status === 404) {
      return Promise.resolve(null);
    }
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

const get = async <T>(path: string): Promise<T> => {
  const response = await axiosInstance.get(path);
  if (response === null) {
    return null as unknown as T;
  }
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