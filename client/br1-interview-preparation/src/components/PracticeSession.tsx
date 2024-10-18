'use client';

import { FC, useEffect, useRef } from 'react';
import { usePracticeSession } from '@/contexts/PracticeSessionContext';
import { useSearchParams } from 'next/navigation';
import Link from 'next/link';
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
    addAnsweredQuestion,
    addSkippedQuestion,
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

    const question = state.currentQuestion;
    if (!question) {
      console.error('Current question is missing or invalid.');
      return;
    }

    const formData = new FormData();
    formData.append('videoFile', videoBlob, 'answer.webm');
    formData.append('questionId', question.id);

    await submitAnswer(formData);
    console.log('The answer was submitted.');

    addAnsweredQuestion(question);
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

    const question = state.currentQuestion;
    if (question) {
      addSkippedQuestion(question);
    }

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

  // TODO: split into components
  if (!state.sessionStarted && state.sessionEndTime) {
    console.log(state);
    const totalTimeSpent =
      state.sessionEndTime - (state.sessionStartTime || state.sessionEndTime);
    console.log(totalTimeSpent);
    const totalTimeSpentSeconds = Math.floor(totalTimeSpent / 1000);
    const minutes = Math.floor(totalTimeSpentSeconds / 60);
    const seconds = totalTimeSpentSeconds % 60;

    return (
      <section>
        <h1 className="text-2xl font-bold">Session Summary</h1>
        <p>
          Total time spent: {minutes} minutes {seconds} seconds
        </p>

        <h2 className="text-xl font-bold">Questions Answered</h2>
        {state.answeredQuestions.length > 0 ? (
          <ul>
            {state.answeredQuestions.map((question, index) => (
              <li key={index}>
                <Link
                  href={`/questions/${question.id}`}
                  className="text-blue-600 underline"
                >
                  {question.content}
                </Link>
              </li>
            ))}
          </ul>
        ) : (
          <p>No questions answered.</p>
        )}

        <h2 className="text-xl font-bold">Questions Skipped</h2>
        {state.skippedQuestions.length > 0 ? (
          <ul>
            {state.skippedQuestions.map((question, index) => (
              <li key={index}>
                <Link
                  href={`/questions/${question.id}`}
                  className="text-blue-600 underline"
                >
                  {question.content}
                </Link>
              </li>
            ))}
          </ul>
        ) : (
          <p>No questions skipped.</p>
        )}

        <button
          onClick={() => {
            startSession();
          }}
          className="bg-blue-600 text-white px-4 py-2 rounded-md"
        >
          Start New Session
        </button>
      </section>
    );
  }

  // TODO: split into components
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

  // TODO: split into components
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
