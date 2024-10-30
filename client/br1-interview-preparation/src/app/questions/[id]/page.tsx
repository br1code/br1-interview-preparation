import { FC } from 'react';
import QuestionDetails from '@/components/questions/QuestionDetails';

interface QuestionProps {
  params: {
    id: string;
  };
}

const Question: FC<QuestionProps> = ({ params }) => {
  const { id } = params;

  return (
    <main className="flex min-h-screen items-center justify-center bg-gray-100 p-4">
      <div className="w-full max-w-3xl bg-white p-8 shadow-md rounded-lg">
        <QuestionDetails questionId={id} />
      </div>
    </main>
  );
};

export default Question;
