'use client';

import PracticeSession from '@/components/practice/PracticeSession';
import { PracticeSessionProvider } from '@/contexts/PracticeSessionContext';

export default function PracticePage() {
  return (
    <PracticeSessionProvider>
      <PracticeSession />
    </PracticeSessionProvider>
  );
}
