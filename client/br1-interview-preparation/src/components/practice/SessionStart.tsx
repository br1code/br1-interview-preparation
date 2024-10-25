import { FC, useEffect } from 'react';
import { useSearchParams } from 'next/navigation';
import { usePracticeSession } from '@/contexts/PracticeSessionContext';
import useFetchCategory from '@/hooks/useFetchCategory';

const SessionStart: FC = () => {
  const searchParams = useSearchParams();
  const categoryId = searchParams.get('categoryId');
  const { startSession, setCategory } = usePracticeSession();

  const {
    category,
    loading: loadingCategory,
    error: categoryError,
  } = useFetchCategory(categoryId);

  useEffect(() => {
    setCategory(category || null);
  }, [category, setCategory]);

  return (
    <section className="text-center">
      <h1 className="text-3xl font-bold mb-4">Practice</h1>
      <p className="text-lg mb-4">
        Welcome to the Practice Session! Here&apos;s how it works:
      </p>
      <ul className="list-disc list-inside text-left mx-auto mb-6 max-w-lg">
        <li>
          You&apos;ll be shown a random question from the selected category.
        </li>
        <li>
          A 5-second countdown will give you time to prepare before recording
          starts.
        </li>
        <li>
          You can choose to view a hint for the question by clicking the
          &quot;Toggle Hint&quot; button.
        </li>
        <li>
          Once the countdown ends, your camera and microphone will start
          recording automatically.
        </li>
        <li>
          After recording your answer, click the &quot;Submit Answer&quot;
          button to upload your response.
        </li>
        <li>
          If you want to skip a question, use the &quot;Skip Question&quot;
          button to move on to the next one.
        </li>
        <li>
          The session continues indefinitely until you decide to stop by
          clicking the &quot;End Session&quot; button.
        </li>
        <li>
          At the end, you&apos;ll see a summary of your session, including the
          number of questions answered and skipped.
        </li>
      </ul>
      {loadingCategory ? (
        <p className="text-lg">Loading category...</p>
      ) : categoryError ? (
        <p className="text-red-500">{categoryError}</p>
      ) : category ? (
        <p className="text-lg mb-4">Selected Category: {category.name}</p>
      ) : (
        <p className="text-lg mb-4">All Categories selected</p>
      )}
      <button
        onClick={startSession}
        className="bg-blue-600 text-white px-6 py-3 rounded-md hover:bg-blue-700 transition"
        disabled={loadingCategory || !!categoryError}
      >
        Start Session
      </button>
    </section>
  );
};

export default SessionStart;
