'use client';

import { FC, useEffect, useRef } from 'react';
import { usePracticeSession } from '@/contexts/PracticeSessionContext';
import { useSearchParams } from 'next/navigation';
import useFetchCategory from '@/hooks/useFetchCategory';
import useMediaRecorder from '@/hooks/useMediaRecorder';
import { submitAnswer } from '@/api';

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
    toggleShowHint,
  } = usePracticeSession();

  const {
    category,
    loading: loadingCategory,
    error: categoryError,
  } = useFetchCategory(categoryId);

  useEffect(() => {
    setCategory(category);
  }, [category, setCategory]);

  const {
    recordedChunksRef,
    permissionError,
    startRecording: startMediaRecording,
    stopRecording: stopMediaRecording,
    resetRecordedChunks,
  } = useMediaRecorder();

  const videoRef = useRef<HTMLVideoElement>(null);

  // Start media recording
  useEffect(() => {
    if (state.isRecording && videoRef.current) {
      startMediaRecording(videoRef.current);
    } else if (!state.isRecording) {
      cleanupVideoPreview();
    }

    return () => {
      cleanupVideoPreview();
    };
  }, [state.isRecording, startMediaRecording]);

  // TODO: split into methods
  const handleSubmitAnswer = async () => {
    stopRecording();
    await stopMediaRecording();

    const recordedChunksData = recordedChunksRef.current;
    const videoBlob = new Blob(recordedChunksData, { type: 'video/webm' });

    if (videoBlob.size === 0) {
      console.error('Video blob is empty.');
      // TODO: Optionally inform the user and prevent submission
      return;
    }

    const questionId = state.currentQuestion?.id;
    if (!questionId) {
      console.error('Question ID is missing or invalid.');
      return;
    }

    const formData = new FormData();
    formData.append('videoFile', videoBlob, 'answer.webm');
    formData.append('questionId', state.currentQuestion?.id || '');

    await submitAnswer(formData);
    console.log('The answer was submitted.');

    resetRecordedChunks();
    fetchNextQuestion();
  };

  const cleanupVideoPreview = () => {
    if (videoRef.current) {
      videoRef.current.pause();
      videoRef.current.srcObject = null;
    }
  };

  // TODO: review
  const handleSkipQuestion = async () => {
    stopRecording();
    await stopMediaRecording();
    resetRecordedChunks();
    cleanupVideoPreview();
    fetchNextQuestion();
  };

  // TODO: review
  const handleEndSession = async () => {
    endSession();
    stopRecording();
    await stopMediaRecording();
    resetRecordedChunks();
    cleanupVideoPreview();
  };

  // TODO: create components
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

              {state.showHint && (
                <p>Hint: {state.currentQuestion.hint || 'No Hint ...'}</p>
              )}

              <button
                onClick={toggleShowHint}
                className="bg-blue-600 text-white px-4 py-2 rounded-md"
              >
                Toggle Hint
              </button>

              <button
                onClick={handleSkipQuestion}
                className="bg-yellow-600 text-white px-4 py-2 rounded-md"
              >
                Skip Question
              </button>
            </div>
          )}

          {state.isCountingDown && <p>Recording in: {state.countdownValue}</p>}

          {state.isRecording && !permissionError && (
            <div>
              <video
                ref={videoRef}
                style={{ width: '100%', maxWidth: '500px' }}
                muted
                autoPlay
              />
              <button
                onClick={handleSubmitAnswer}
                className="bg-green-600 text-white px-4 py-2 rounded-md"
              >
                Submit Answer
              </button>
            </div>
          )}

          {permissionError && <p className="text-red-500">{permissionError}</p>}

          {!state.isCountingDown && !state.isRecording && (
            <p>Preparing next question...</p>
          )}
        </>
      )}

      <button
        onClick={handleEndSession}
        className="bg-red-600 text-white px-4 py-2 rounded-md"
      >
        End Session
      </button>
    </section>
  );
};

export default PracticeSession;
