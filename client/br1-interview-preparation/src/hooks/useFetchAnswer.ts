import { useEffect, useState } from 'react';
import { fetchAnswerMetadata } from '@/api';
import { Answer } from '@/types';

interface UseFetchAnswerResult {
  answer: Answer | null;
  loading: boolean;
  error: string | null;
}

const useFetchAnswer = (answerId: string): UseFetchAnswerResult => {
  const [answer, setAnswer] = useState<Answer | null>(null);
  const [loading, setLoading] = useState<boolean>(true); // initialized to true on purpose
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchAndSetAnswer = async () => {
      try {
        setError(null);
        const fetchedAnswer = await fetchAnswerMetadata(answerId);
        setAnswer(fetchedAnswer);
      } catch (error) {
        console.error('Error fetching answer:', error);
        setError('Failed to load answer.');
        setAnswer(null);
      } finally {
        setLoading(false);
      }
    };

    fetchAndSetAnswer();
  }, [answerId]);

  return { answer, loading, error };
};

export default useFetchAnswer;
