import { FrequencyType, TMovement } from '../models/movement.models';
import { months } from './months';

export const hasFrequency = (movement: TMovement): boolean => {
  return movement.frequencyType !== FrequencyType.None &&
    movement.frequencyType !== FrequencyType.Undefined;
};

export const getFrequency = (movement: TMovement): string[] => {
  if (!hasFrequency(movement)) {
    return [];
  }

  if (movement.frequencyType === FrequencyType.Monthly) {
    return ['men'];
  }

  if (movement.frequencyType === FrequencyType.Yearly) {
    return [months[movement.frequencyMonth - 1]];
  }

  return movement.frequencyMonths
    .map((selected: boolean, index: number) => selected ? index : 12)
    .filter((value: number) => value !== 12)
    .map((value: number) => months[value]);
};