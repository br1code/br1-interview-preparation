import { z } from 'zod';

export const categorySchema = z.object({
  id: z.string(),
  name: z.string(),
});

export const categoriesSchema = z.array(categorySchema);

export type Category = z.infer<typeof categorySchema>;

export const questionSchema = z.object({
  id: z.string(),
  categoryId: z.string(),
  content: z.string(),
  hint: z.string().optional(),
});

export const questionsSchema = z.array(questionSchema);

export type Question = z.infer<typeof questionSchema>;

export interface DropdownOption {
  value: string;
  label: string;
}
