import { z } from 'zod';

export const categorySchema = z.object({
  id: z.string(),
  name: z.string(),
});

export const categoriesSchema = z.array(categorySchema);

export type Category = z.infer<typeof categorySchema>;

export interface DropdownOption {
  value: string;
  label: string;
}
