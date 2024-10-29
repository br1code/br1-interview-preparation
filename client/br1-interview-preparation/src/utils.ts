import { DropdownOption } from './types';

export const toDropdownOptions = (
  source: { id: string; name: string }[]
): DropdownOption[] => {
  return source.map((item) => ({
    label: item.name,
    value: item.id,
  }));
};

export const getUniqueItemsWithCount = <T, K extends keyof T>(
  array: T[],
  key: K
): Array<{ item: T; count: number }> => {
  const map = new Map<T[K], { item: T; count: number }>();

  array.forEach((item) => {
    const keyValue = item[key];
    if (map.has(keyValue)) {
      map.get(keyValue)!.count += 1;
    } else {
      map.set(keyValue, { item, count: 1 });
    }
  });

  return Array.from(map.values());
};
