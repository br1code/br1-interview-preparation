'use client';

import { FC, useEffect } from 'react';
import { usePracticeSession } from '@/contexts/PracticeSessionContext';
import { useSearchParams } from 'next/navigation';

const PracticeSession: FC = () => {
  const searchParams = useSearchParams();
  const categoryId = searchParams.get('categoryId');
  const { state, setCategoryId } = usePracticeSession();

  useEffect(() => {
    if (categoryId !== state.categoryId) {
      setCategoryId(categoryId);
    }
  }, [categoryId, state.categoryId, setCategoryId]);

  return (
    <section>
      <h1 className="text-2xl font-bold mb-4">Practice</h1>

      {state.loadingQuestion ? (
        <p>Loading question...</p>
      ) : state.error ? (
        <p className="text-red-500">{state.error}</p>
      ) : state.currentQuestion ? (
        <div>
          <h2 className="text-xl font-semibold">Question:</h2>
          <p>{state.currentQuestion.content}</p>
        </div>
      ) : (
        <p>No question available.</p>
      )}
    </section>
  );
};

export default PracticeSession;
