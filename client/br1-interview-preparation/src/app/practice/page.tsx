'use client';

import PracticeSession from '@/components/practice/PracticeSession';
import { PracticeSessionProvider } from '@/contexts/PracticeSessionContext';

export default function Practice() {
  return (
    <main>
      <PracticeSessionProvider>
        <PracticeSession />
      </PracticeSessionProvider>
    </main>
  );
}
