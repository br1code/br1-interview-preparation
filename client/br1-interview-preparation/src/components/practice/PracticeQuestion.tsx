import { FC, useEffect, useRef } from 'react';
import { usePracticeSession } from '@/contexts/PracticeSessionContext';
import { submitAnswer } from '@/api';
import useMediaRecorder from '@/hooks/useMediaRecorder';

const PracticeQuestion: FC = () => {
  const {
    state,
    stopRecording: stopSessionRecording,
    fetchNextQuestion,
    toggleShowHint,
    addAnsweredQuestion,
    addSkippedQuestion,
    endSession,
  } = usePracticeSession();

  const {
    recordedChunksRef,
    permissionError,
    startRecording: startMediaRecording,
    stopRecording: stopMediaRecording,
    resetRecordedChunks,
  } = useMediaRecorder();

  const videoRef = useRef<HTMLVideoElement>(null);

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

  const cleanupVideoPreview = () => {
    if (videoRef.current) {
      videoRef.current.pause();
      videoRef.current.srcObject = null;
    }
  };

  const handleSubmitAnswer = async () => {
    stopSessionRecording();
    await stopMediaRecording();

    const recordedChunksData = recordedChunksRef.current;
    const videoBlob = new Blob(recordedChunksData, { type: 'video/webm' });

    if (videoBlob.size === 0) {
      console.error('Video blob is empty.');
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

  const handleSkipQuestion = async () => {
    stopSessionRecording();
    await stopMediaRecording();
    resetRecordedChunks();
    cleanupVideoPreview();

    const question = state.currentQuestion;
    if (question) {
      addSkippedQuestion(question);
    }

    fetchNextQuestion();
  };

  const handleEndSession = async () => {
    endSession();
    stopSessionRecording();
    await stopMediaRecording();
    resetRecordedChunks();
    cleanupVideoPreview();
  };

  if (state.loadingQuestion) {
    return (
      <section className="text-center">
        <p className="text-lg">Loading question...</p>
      </section>
    );
  }

  if (state.error) {
    return (
      <section className="text-center">
        <p className="text-red-500">{state.error}</p>
      </section>
    );
  }

  return (
    <section className="text-center">
      <h1 className="text-3xl font-bold mb-4">Practice Session</h1>
      {state.category && (
        <p className="text-lg mb-4">Selected Category: {state.category.name}</p>
      )}

      {state.currentQuestion && (
        <div className="mb-6">
          <h2 className="text-2xl font-semibold mb-2">Question:</h2>
          <p className="text-lg mb-4">{state.currentQuestion.content}</p>

          {state.showHint && (
            <p className="text-yellow-600 mb-4">
              Hint: {state.currentQuestion.hint || 'No Hint ...'}
            </p>
          )}

          <button
            onClick={toggleShowHint}
            className="bg-blue-600 text-white px-6 py-3 rounded-md hover:bg-blue-700 transition mb-4"
          >
            Toggle Hint
          </button>

          <button
            onClick={handleSkipQuestion}
            className="bg-yellow-600 text-white px-6 py-3 rounded-md hover:bg-yellow-700 transition mb-4"
          >
            Skip Question
          </button>
        </div>
      )}

      {state.isCountingDown && (
        <p className="text-lg mb-4">Recording in: {state.countdownValue}</p>
      )}

      {state.isRecording && !permissionError && (
        <div className="mb-6">
          <video
            ref={videoRef}
            className="w-full max-w-lg mx-auto mb-4 border-2 border-gray-300"
            muted
            autoPlay
          />
          <button
            onClick={handleSubmitAnswer}
            className="bg-green-600 text-white px-6 py-3 rounded-md hover:bg-green-700 transition"
          >
            Submit Answer
          </button>
        </div>
      )}

      {permissionError && (
        <p className="text-red-500 mb-4">{permissionError}</p>
      )}

      {!state.isCountingDown && !state.isRecording && (
        <p className="text-lg mb-4">Preparing next question...</p>
      )}

      <button
        onClick={handleEndSession}
        className="bg-red-600 text-white px-6 py-3 rounded-md hover:bg-red-700 transition"
      >
        End Session
      </button>
    </section>
  );
};

export default PracticeQuestion;
