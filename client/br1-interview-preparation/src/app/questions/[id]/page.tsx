import { FC } from 'react';
import QuestionDetails from '@/components/questions/QuestionDetails';

interface QuestionPageProps {
  params: {
    id: string;
  };
}

const QuestionPage: FC<QuestionPageProps> = ({ params }) => {
  const { id } = params;

  return (
    <main className="flex min-h-[calc(100vh-4rem)] items-center justify-center bg-gray-100 p-4">
      <QuestionDetails questionId={id} />
    </main>
  );
};

export default QuestionPage;
