import { FC } from 'react';
import AddQuestionForm from '@/components/questions/AddQuestionForm';
import Link from 'next/link';

const AddQuestion: FC = () => {
  return (
    <main className="flex min-h-screen items-center justify-center bg-gray-100 p-4">
      <div className="w-full max-w-3xl bg-white p-8 shadow-md rounded-lg">
        <h1 className="text-3xl font-bold mb-6 text-center">Add Question</h1>
        <AddQuestionForm />
        <div className="text-center">
          <Link
            href="/"
            className="text-blue-600 underline hover:text-blue-800"
          >
            Return to Homepage
          </Link>
        </div>
      </div>
    </main>
  );
};

export default AddQuestion;
