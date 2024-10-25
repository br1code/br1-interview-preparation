import { FC } from 'react';
import Link from 'next/link';
import { usePracticeSession } from '@/contexts/PracticeSessionContext';

const SessionSummary: FC = () => {
  const { state, startSession } = usePracticeSession();

  const totalTimeSpent =
    (state.sessionEndTime || 0) -
    (state.sessionStartTime || state.sessionEndTime || 0);
  const totalTimeSpentSeconds = Math.floor(totalTimeSpent / 1000);
  const minutes = Math.floor(totalTimeSpentSeconds / 60);
  const seconds = totalTimeSpentSeconds % 60;

  return (
    <section className="text-center">
      <h1 className="text-3xl font-bold mb-4">Session Summary</h1>
      <p className="text-lg mb-6">
        Total time spent: {minutes} minutes {seconds} seconds
      </p>

      <h2 className="text-2xl font-semibold mb-2">Questions Answered</h2>
      {state.answeredQuestions.length > 0 ? (
        <ul className="list-disc list-inside mb-4">
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
        <p className="mb-4">No questions answered.</p>
      )}

      <h2 className="text-2xl font-semibold mb-2">Questions Skipped</h2>
      {state.skippedQuestions.length > 0 ? (
        <ul className="list-disc list-inside mb-6">
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
        <p className="mb-6">No questions skipped.</p>
      )}

      <button
        onClick={startSession}
        className="bg-blue-600 text-white px-6 py-3 rounded-md hover:bg-blue-700 transition"
      >
        Start New Session
      </button>
    </section>
  );
};

export default SessionSummary;
