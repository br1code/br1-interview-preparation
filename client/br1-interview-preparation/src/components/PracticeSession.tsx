'use client';

import { FC, useEffect } from 'react';
import { usePracticeSession } from '@/contexts/PracticeSessionContext';
import { useSearchParams } from 'next/navigation';
import useFetchCategory from '@/hooks/useFetchCategory';

const PracticeSession: FC = () => {
  const searchParams = useSearchParams();
  const categoryId = searchParams.get('categoryId');
  const {
    state,
    setCategory,
    startSession,
    endSession,
    stopRecording,
    fetchNextQuestion,
  } = usePracticeSession();

  const {
    category,
    loading: loadingCategory,
    error: categoryError,
  } = useFetchCategory(categoryId);

  useEffect(() => {
    setCategory(category);
  }, [category, setCategory]);

  // TODO: Implement the logic to submit the recorded video
  const submitVideo = async () => {
    return new Promise<void>((resolve) => {
      console.log('Submitting video');
      setTimeout(() => {
        console.log('Video submitted');
        resolve();
      }, 1000);
    });
  };

  const onSubmitAnswerClick = async () => {
    stopRecording();
    await submitVideo();
    console.log('Done!, next question');
    fetchNextQuestion();
  };

  if (!state.sessionStarted) {
    return (
      <section>
        <h1 className="text-2xl font-bold">Practice</h1>
        <p>How it works: TODO - explain all the rules</p>
        {loadingCategory ? (
          <p>Loading category...</p>
        ) : categoryError ? (
          <p className="text-red-500">{categoryError}</p>
        ) : category ? (
          <p>Selected Category: {category.name}</p>
        ) : (
          <p>All Categories selected</p>
        )}
        <button
          onClick={startSession}
          className="bg-blue-600 text-white px-4 py-2 rounded-md"
          disabled={loadingCategory || !!categoryError}
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
      {category ? <p>Selected Category: {category.name}</p> : ''}
      {state.loadingQuestion ? (
        <p>Loading question...</p>
      ) : state.error ? (
        <p className="text-red-500">{state.error}</p>
      ) : (
        <>
          {state.currentQuestion && (
            <div>
              <h2>Question:</h2>
              <p>{state.currentQuestion.content}</p>
            </div>
          )}

          {state.isCountingDown && <p>Starting in: {state.countdownValue}</p>}

          {state.isRecording && (
            <div>
              <p>Recording...</p>
              {/* Add a video preview if desired */}
              <button
                onClick={onSubmitAnswerClick}
                className="bg-green-600 text-white px-4 py-2 rounded-md"
              >
                Submit Answer
              </button>
            </div>
          )}

          {!state.isCountingDown && !state.isRecording && (
            <p>Preparing next question...</p>
          )}
        </>
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
