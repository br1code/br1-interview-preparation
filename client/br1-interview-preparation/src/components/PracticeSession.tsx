'use client';

import { FC, useEffect } from 'react';
import { usePracticeSession } from '@/contexts/PracticeSessionContext';
import { useSearchParams } from 'next/navigation';

const PracticeSession: FC = () => {
  const searchParams = useSearchParams();
  const categoryId = searchParams.get('categoryId');
  const { state, setCategoryId, startSession, endSession } =
    usePracticeSession();

  useEffect(() => {
    if (categoryId !== state.categoryId) {
      setCategoryId(categoryId);
    }
  }, [categoryId, state.categoryId, setCategoryId]);

  if (!state.sessionStarted) {
    return (
      <section>
        <h1 className="text-2xl font-bold">Practice</h1>
        <p>How it works: TODO - explain all the rules</p>
        {state.categoryId ? (
          <p>Selected Category ID: {state.categoryId}</p>
        ) : (
          <p>All Categories selected</p>
        )}
        <button
          onClick={startSession}
          className="bg-blue-600 text-white px-4 py-2 rounded-md"
        >
          Start Session
        </button>
      </section>
    );
  }

  // TODO: create components
  return (
    <section>
      <h1 className="text-2xl font-bold">Practice Session</h1>
      {state.loadingQuestion ? (
        <p>Loading question...</p>
      ) : state.error ? (
        <p className="text-red-500">{state.error}</p>
      ) : state.currentQuestion ? (
        <div>
          <h2>Question:</h2>
          <p>{state.currentQuestion.content}</p>
        </div>
      ) : (
        <p>No question available.</p>
      )}

      {/* Include other components like HintButton, RecordingControls, etc. */}
      <button
        onClick={endSession}
        className="bg-red-600 text-white px-4 py-2 rounded-md"
      >
        End Session
      </button>
    </section>
  );
};

export default PracticeSession;
