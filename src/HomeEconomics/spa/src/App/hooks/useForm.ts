import { ChangeEvent, useState } from 'react';

type UseForm<TValues> = {
  handleChange: (event: ChangeEvent<HTMLInput>) => void;
  handleSubmit: (event: React.FormEvent<HTMLFormElement>) => void;
  values: TValues;
  setValues: React.Dispatch<React.SetStateAction<TValues>>;
}

type HTMLInput = HTMLInputElement | HTMLSelectElement;

type StringIndexed = {
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  [key: string]: any;
}

const useForm = <TValues extends StringIndexed>(initialValues: TValues, callback: Function = (): void => { return }): UseForm<TValues> => {

  const [values, setValues] = useState<TValues>(initialValues);

  const handleSubmit = (event: React.FormEvent<HTMLFormElement>): void => {
    if (event) event.preventDefault();
    callback();
  };

  const propertyIsNumber = (name: string): boolean => {
    return typeof values[name] === 'number';
  }

  const handleChange = (event: ChangeEvent<HTMLInput>): void => {
    event.persist();
    const value = propertyIsNumber(event.target.name)
      ? parseInt(event.target.value)
      : event.target.value;
    setValues(values => ({ ...values, [event.target.name]: value }));
  };

  return {
    handleChange,
    handleSubmit,
    values,
    setValues
  }
};

export default useForm;