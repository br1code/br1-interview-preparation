import { useEffect, useState } from 'react';
import { fetchQuestions } from '@/api';
import { QuestionSummary } from '@/types';

interface UseFetchQuestionsResult {
  questions: QuestionSummary[] | null;
  loading: boolean;
  error: string | null;
}

interface FetchQuestionsParams {
  categoryId?: string | null;
  pageNumber?: number;
  pageSize?: number;
  content?: string;
}

const useFetchQuestions = (
  {
    categoryId = null,
    pageNumber = 1,
    pageSize = 10,
    content = '',
  }: FetchQuestionsParams,
  refreshKey?: number
): UseFetchQuestionsResult => {
  const [questions, setQuestions] = useState<QuestionSummary[] | null>(null);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchAndSetQuestions = async () => {
      try {
        setLoading(true);
        setError(null);
        setQuestions(null);

        const fetchedQuestions = await fetchQuestions({
          categoryId,
          pageNumber,
          pageSize,
          content,
        });

        setQuestions(fetchedQuestions);
      } catch (error) {
        console.error('Error fetching questions:', error);
        setError('Failed to load questions.');
        setQuestions(null);
      } finally {
        setLoading(false);
      }
    };

    fetchAndSetQuestions();
  }, [categoryId, pageNumber, pageSize, content, refreshKey]);

  return { questions, loading, error };
};

export default useFetchQuestions;
