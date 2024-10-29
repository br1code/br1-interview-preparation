import { FC } from 'react';
import Link from 'next/link';

const Questions: FC = () => {
  return (
    <main>
      {/* TODO: IMPLEMENT THIS PAGE AND PLACE THIS BUTTON AT THE TOP */}
      <Link href="/questions/add">
        <button className="bg-green-600 text-white px-6 py-3 rounded-md hover:bg-green-700 transition mt-4">
          Add Question
        </button>
      </Link>
    </main>
  );
};

export default Questions;
