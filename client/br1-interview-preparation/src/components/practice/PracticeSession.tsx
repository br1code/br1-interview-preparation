'use client';

import { FC, Suspense } from 'react';
import { usePracticeSession } from '@/contexts/PracticeSessionContext';
import SessionSummary from '@/components/practice/SessionSummary';
import SessionStart from '@/components/practice/SessionStart';
import PracticeQuestion from '@/components/practice/PracticeQuestion';

const PracticeSession: FC = () => {
  const { state } = usePracticeSession();

  return (
    <main className="flex min-h-[calc(100vh-4rem)] items-center justify-center bg-gray-100 p-4">
      <div className="w-full max-w-2xl bg-white p-8 shadow-md rounded-lg">
        {!state.sessionStarted && state.sessionEndTime ? (
          <SessionSummary />
        ) : !state.sessionStarted ? (
          <Suspense fallback={<div>Loading...</div>}>
            <SessionStart />
          </Suspense>
        ) : (
          <PracticeQuestion />
        )}
      </div>
    </main>
  );
};

export default PracticeSession;
