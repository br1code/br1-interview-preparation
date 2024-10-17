import { fetchData, postData } from './http';
import {
  categoriesSchema,
  Category,
  categorySchema,
  Question,
  questionSchema,
  questionsSchema,
  submittedAnswerIdSchema,
} from './types';

export const fetchCategories = (): Promise<Category[]> => {
  return fetchData('categories', categoriesSchema);
};

export const fetchCategory = (categoryId: string): Promise<Category> => {
  return fetchData(`categories/${categoryId}`, categorySchema);
};

export const fetchQuestions = (categoryId?: string): Promise<Question[]> => {
  const searchQuery = categoryId ? `?categoryId=${categoryId}` : '';
  return fetchData(`questions${searchQuery}`, questionsSchema);
};

export const fetchRandomQuestion = (
  categoryId?: string | null
): Promise<Question> => {
  const searchQuery = categoryId ? `?categoryId=${categoryId}` : '';
  return fetchData(`questions/random${searchQuery}`, questionSchema);
};

export const submitAnswer = (data: FormData): Promise<string> => {
  return postData('answers', submittedAnswerIdSchema, data);
};
