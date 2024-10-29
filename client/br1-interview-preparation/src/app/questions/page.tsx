import { FC } from 'react';
import Link from 'next/link';
import { fetchCategories } from '@/api';
import QuestionsList from '@/components/questions/QuestionsList';
import { toDropdownOptions } from '@/utils';

const Questions: FC = async () => {
  const categories = await fetchCategories();

  return (
    <main className="flex flex-col items-center p-8">
      <Link href="/questions/add">
        <button className="bg-green-600 text-white px-6 py-3 rounded-md hover:bg-green-700 transition mb-6">
          Add Question
        </button>
      </Link>

      <QuestionsList categoriesOptions={toDropdownOptions(categories)} />

      <div className="text-center mt-4">
        <Link href="/" className="text-blue-600 underline hover:text-blue-800">
          Return to Homepage
        </Link>
      </div>
    </main>
  );
};

export default Questions;
