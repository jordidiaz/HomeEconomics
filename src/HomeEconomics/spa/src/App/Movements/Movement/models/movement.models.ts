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