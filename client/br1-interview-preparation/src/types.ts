import { z } from 'zod';

// Interfaces
export interface DropdownOption {
  value: string;
  label: string;
}

// Schemas
export const categorySchema = z.object({
  id: z.string(),
  name: z.string(),
});
export const categoriesSchema = z.array(categorySchema);

export const categoryDetailsSchema = z.object({
  id: z.string(),
  name: z.string(),
  questionsCount: z.number(),
});
export const detailedCategoriesSchema = z.array(categoryDetailsSchema);

export const addCategorySchema = z.object({
  name: z.string(),
});

export const questionSchema = z.object({
  id: z.string(),
  categoryId: z.string(),
  content: z.string(),
  hint: z.string().optional(),
});

export const questionSummarySchema = z.object({
  id: z.string(),
  categoryId: z.string(),
  content: z.string(),
  hint: z.string().optional(),
  answersCount: z.number(),
});

export const questionSummariesSchema = z.array(questionSummarySchema);

export const addQuestionSchema = z.object({
  categoryId: z.string(),
  content: z.string(),
  hint: z.string().optional(),
});

export const answerSchema = z.object({
  id: z.string(),
  questionId: z.string(),
  videoFilename: z.string(),
  createdAt: z.string(),
});

export const questionWithAnswersSchema = z.object({
  id: z.string(),
  categoryId: z.string(),
  content: z.string(),
  hint: z.string().optional(),
  answers: z.array(answerSchema).optional(),
});

export const createdEntityIdSchema = z.string();

// Types
export type Category = z.infer<typeof categorySchema>;
export type CategoryDetails = z.infer<typeof categoryDetailsSchema>;
export type AddCategory = z.infer<typeof addCategorySchema>;
export type Question = z.infer<typeof questionSchema>;
export type QuestionSummary = z.infer<typeof questionSummarySchema>;
export type AddQuestion = z.infer<typeof addQuestionSchema>;
export type Answer = z.infer<typeof answerSchema>;
export type QuestionWithAnswers = z.infer<typeof questionWithAnswersSchema>;
