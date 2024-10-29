import { deleteData, fetchData, postData, putData } from './http';
import {
  Answer,
  answerSchema,
  categoriesSchema,
  Category,
  categorySchema,
  Question,
  questionSchema,
  QuestionWithAnswers,
  questionWithAnswersSchema,
  createdEntityIdSchema,
  AddQuestion,
  QuestionSummary,
  questionSummariesSchema,
} from './types';

// Categories

export const fetchCategories = (): Promise<Category[]> => {
  return fetchData('categories', categoriesSchema);
};

export const fetchCategory = (categoryId: string): Promise<Category> => {
  return fetchData(`categories/${categoryId}`, categorySchema);
};

// Questions

export const fetchQuestions = (
  categoryId?: string | null
): Promise<QuestionSummary[]> => {
  const searchQuery = categoryId ? `?categoryId=${categoryId}` : '';
  return fetchData(`questions${searchQuery}`, questionSummariesSchema);
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

export const updateQuestion = (
  questionId: string,
  data: Omit<Question, 'id'>
): Promise<Question> => {
  return putData(`questions/${questionId}`, data);
};

export const deleteQuestion = (questionId: string): Promise<void> => {
  return deleteData(`questions/${questionId}`);
};

export const addQuestion = (data: AddQuestion): Promise<string> => {
  return postData('questions', createdEntityIdSchema, data);
};

// Answers

export const submitAnswer = (data: FormData): Promise<string> => {
  return postData('answers', createdEntityIdSchema, data);
};

export const fetchAnswerMetadata = (answerId: string): Promise<Answer> => {
  return fetchData(`answers/${answerId}/metadata`, answerSchema);
};

export const deleteAnswer = (answerId: string): Promise<void> => {
  return deleteData(`answers/${answerId}`);
};
