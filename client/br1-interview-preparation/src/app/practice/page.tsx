'use client';

import PracticeSession from '@/components/PracticeSession';
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
