export type TMovement = {
  id: number;
  name: string;
  amount: number;
  type: MovementType;
  frequencyType: FrequencyType;
  frequencyMonths: boolean[];
}

export enum MovementType {
  Income,
  Expense
}

export enum FrequencyType {
  None,
  Monthly,
  Yearly,
  Custom
}