import { DropdownOption } from './types';

export const toDropdownOptions = (
  source: { id: string; name: string }[]
): DropdownOption[] => {
  return source.map((item) => ({
    label: item.name,
    value: item.id,
  }));
};
