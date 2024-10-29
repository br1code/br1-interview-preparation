import { FC } from 'react';
import { fetchCategories } from '@/api';
import AddQuestionForm from '@/components/questions/AddQuestionForm';
import { toDropdownOptions } from '@/utils';

const AddQuestion: FC = async () => {
  const categories = await fetchCategories();
  return (
    <main className="flex min-h-screen items-center justify-center bg-gray-100 p-4">
      <div className="w-full max-w-3xl bg-white p-8 shadow-md rounded-lg">
        <h1 className="text-3xl font-bold mb-6 text-center">Add Question</h1>
        <AddQuestionForm categoriesOptions={toDropdownOptions(categories)} />
      </div>
    </main>
  );
};

export default AddQuestion;
