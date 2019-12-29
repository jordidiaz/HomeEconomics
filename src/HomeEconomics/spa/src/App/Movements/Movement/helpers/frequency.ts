import { FrequencyType, TMovement } from '../models/movement.models';
import { months } from './months';

export const hasFrequency = (movement: TMovement): boolean => {
  return movement.frequencyType !== FrequencyType.None;
};

export const getFrequency = (movement: TMovement): string[] => {
  if (!hasFrequency(movement) || movement.frequencyMonths.length > 12) {
    return [];
  }

  return movement.frequencyType === FrequencyType.Monthly
    ? ['men']
    : movement.frequencyMonths
      .map((selected: boolean, index: number) => selected ? index : 12)
      .filter((value: number) => value !== 12)
      .map((value: number) => months[value]);
};