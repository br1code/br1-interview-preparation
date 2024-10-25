'use client';

import { FC } from 'react';
import Link from 'next/link';
import { usePracticeSession } from '@/contexts/PracticeSessionContext';
import SessionSummary from '@/components/practice/SessionSummary';
import SessionStart from '@/components/practice/SessionStart';
import PracticeQuestion from '@/components/practice/PracticeQuestion';

const PracticeSession: FC = () => {
  const { state } = usePracticeSession();

  return (
    <main className="flex min-h-screen items-center justify-center bg-gray-100 p-4">
      <div className="w-full max-w-2xl bg-white p-8 shadow-md rounded-lg">
        {!state.sessionStarted && state.sessionEndTime ? (
          <SessionSummary />
        ) : !state.sessionStarted ? (
          <SessionStart />
        ) : (
          <PracticeQuestion />
        )}
        <div className="mt-8 text-center">
          <Link
            href="/"
            className="text-blue-600 underline hover:text-blue-800"
          >
            Return to Homepage
          </Link>
        </div>
      </div>
    </main>
  );
};

export default PracticeSession;
