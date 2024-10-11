import { fetchData } from './api';
import { categoriesSchema, Category } from './types';

export const fetchCategories = (): Promise<Category[]> => {
  return fetchData('categories', categoriesSchema);
};
