import { useEffect, useState } from 'react';
import { fetchQuestion } from '@/api';
import { QuestionWithAnswers } from '@/types';

interface UseFetchQuestionResult {
  question: QuestionWithAnswers | null;
  loading: boolean;
  error: string | null;
}

const useFetchQuestion = (questionId: string): UseFetchQuestionResult => {
  const [question, setQuestion] = useState<QuestionWithAnswers | null>(null);
  const [loading, setLoading] = useState<boolean>(true); // initialized to true on purpose
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchAndSetQuestion = async () => {
      try {
        setLoading(true);
        setError(null);
        const fetchedQuestion = await fetchQuestion(questionId);
        setQuestion(fetchedQuestion);
      } catch (error) {
        console.error('Error fetching question:', error);
        setError('Failed to load question.');
        setQuestion(null);
      } finally {
        setLoading(false);
      }
    };

    fetchAndSetQuestion();
  }, [questionId]);

  return { question, loading, error };
};

export default useFetchQuestion;
