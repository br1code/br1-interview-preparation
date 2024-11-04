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
  detailedCategoriesSchema,
  CategoryDetails,
  AddCategory,
} from './types';

// Categories

export const fetchCategories = (): Promise<Category[]> => {
  return fetchData('categories', categoriesSchema);
};

export const fetchDetailedCategories = (): Promise<CategoryDetails[]> => {
  return fetchData('categories/detailed', detailedCategoriesSchema);
};

export const deleteCategory = (categoryId: string): Promise<void> => {
  return deleteData(`categories/${categoryId}`);
};

export const fetchCategory = (categoryId: string): Promise<Category> => {
  return fetchData(`categories/${categoryId}`, categorySchema);
};

export const addCategory = (data: AddCategory): Promise<string> => {
  return postData('categories', createdEntityIdSchema, data);
};

export const updateCategory = (
  categoryId: string,
  data: Omit<Category, 'id'>
): Promise<Category> => {
  return putData(`categories/${categoryId}`, data);
};

// Questions

export const fetchQuestions = (params: {
  categoryId?: string | null;
  pageNumber?: number;
  pageSize?: number;
  content?: string;
}): Promise<QuestionSummary[]> => {
  const { categoryId, pageNumber = 1, pageSize = 10, content = '' } = params;

  const searchParams = new URLSearchParams();

  if (categoryId) {
    searchParams.append('categoryId', categoryId);
  }
  if (pageNumber) {
    searchParams.append('pageNumber', pageNumber.toString());
  }
  if (pageSize) {
    searchParams.append('pageSize', pageSize.toString());
  }
  if (content) {
    searchParams.append('content', content);
  }

  const searchQuery = searchParams.toString()
    ? `?${searchParams.toString()}`
    : '';

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
