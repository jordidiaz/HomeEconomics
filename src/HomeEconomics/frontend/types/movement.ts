import type { FrequencyType } from "./frequency-type";
import type { MovementType } from "./movement-type";

export type Movement = {
  id: number;
  name: string;
  amount: number;
  type: MovementType;
  frequencyType: FrequencyType;
  frequencyMonth: number;
  frequencyMonths: boolean[];
};
