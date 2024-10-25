import { FC } from 'react';
import { fetchCategories } from '@/api';
import QuestionDetails from '@/components/questions/QuestionDetails';
import { toDropdownOptions } from '@/utils';

interface QuestionProps {
  params: {
    id: string;
  };
}

const Question: FC<QuestionProps> = async ({ params }) => {
  const { id } = params;
  const categories = await fetchCategories();

  return (
    <main className="flex min-h-screen items-center justify-center bg-gray-100 p-4">
      <div className="w-full max-w-3xl bg-white p-8 shadow-md rounded-lg">
        <h1 className="text-4xl font-bold mb-6 text-center">
          Question Details
        </h1>
        <QuestionDetails
          questionId={id}
          categoriesOptions={toDropdownOptions(categories)}
        />
      </div>
    </main>
  );
};

export default Question;
