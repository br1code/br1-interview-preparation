import { useEffect, useState } from 'react';
import { fetchQuestions } from '@/api';
import { QuestionSummary } from '@/types';

interface UseFetchQuestionsResult {
  questions: QuestionSummary[] | null;
  loading: boolean;
  error: string | null;
}

const useFetchQuestions = (
  categoryId?: string | null,
  refreshKey?: number
): UseFetchQuestionsResult => {
  const [questions, setQuestions] = useState<QuestionSummary[] | null>(null);
  const [loading, setLoading] = useState<boolean>(true); // initialized to true on purpose
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchAndSetQuestions = async () => {
      try {
        setLoading(true);
        setError(null);
        setQuestions(null);
        const fetchedQuestions = await fetchQuestions(categoryId);
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
  }, [categoryId, refreshKey]);

  return { questions, loading, error };
};

export default useFetchQuestions;
