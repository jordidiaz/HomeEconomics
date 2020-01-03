export type TMovement = {
  id: number;
  name: string;
  amount: number;
  type: MovementType;
  frequencyType: FrequencyType;
  frequencyMonth: number;
  frequencyMonths: boolean[];
}

export enum MovementType {
  Undefined = -1,
  Income,
  Expense
}

export enum FrequencyType {
  Undefined = -1,
  None,
  Monthly,
  Yearly,
  Custom
}

export const emptyMovement: TMovement = {
  id: -1,
  name: '',
  amount: 0,
  type: MovementType.Undefined,
  frequencyType: FrequencyType.Undefined,
  frequencyMonth: -1,
  frequencyMonths: Array.from({ length: 12 }, () => false)
};
