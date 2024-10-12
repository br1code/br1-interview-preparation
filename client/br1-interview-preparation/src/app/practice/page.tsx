'use client';

import { fetchRandomQuestion } from '@/api';
import { useSearchParams } from 'next/navigation';
import { useEffect } from 'react';

export default function Practice() {
  const searchParams = useSearchParams();
  const categoryId = searchParams.get('categoryId');

  // TODO: create custom hook, include loading and error states
  useEffect(() => {
    const getRandomQuestion = async () => {
      try {
        const question = await fetchRandomQuestion(categoryId || '');
        console.log(question);
      } catch (error) {
        console.error(error);
      }
    };

    getRandomQuestion();
  }, [categoryId]);

  return (
    <main>
      <h1>Practice</h1>
      {categoryId ? (
        <p>Selected Category ID: {categoryId}</p>
      ) : (
        <p>All Categories selected</p>
      )}
    </main>
  );
}
