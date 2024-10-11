'use client';

import { useSearchParams } from 'next/navigation';

export default function Practice() {
  const searchParams = useSearchParams();
  const categoryId = searchParams.get('categoryId');

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
