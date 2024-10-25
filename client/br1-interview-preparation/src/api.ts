import { deleteData, fetchData, postData, putData } from './http';
import {
  Answer,
  answerSchema,
  categoriesSchema,
  Category,
  categorySchema,
  Question,
  questionSchema,
  questionsSchema,
  QuestionWithAnswers,
  questionWithAnswersSchema,
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

export const fetchQuestion = (
  questionId: string
): Promise<QuestionWithAnswers> => {
  return fetchData(`questions/${questionId}`, questionWithAnswersSchema);
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

export const updateQuestion = (
  questionId: string,
  data: Omit<Question, 'id'>
): Promise<Question> => {
  return putData(`questions/${questionId}`, data);
};

export const deleteQuestion = (questionId: string): Promise<void> => {
  return deleteData(`questions/${questionId}`);
};

export const fetchAnswerMetadata = (answerId: string): Promise<Answer> => {
  return fetchData(`answers/${answerId}/metadata`, answerSchema);
};

export const deleteAnswer = (answerId: string): Promise<void> => {
  return deleteData(`answers/${answerId}`);
};
