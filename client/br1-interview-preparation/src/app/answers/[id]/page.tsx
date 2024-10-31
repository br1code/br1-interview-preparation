import { FC } from 'react';
import AnswerDetails from '@/components/answers/AnswerDetails';

interface AnswerPageProps {
  params: {
    id: string;
  };
}

const AnswerPage: FC<AnswerPageProps> = ({ params }) => {
  const { id } = params;

  return (
    <main className="flex min-h-[calc(100vh-4rem)] items-center justify-center bg-gray-100 p-4">
      <AnswerDetails answerId={id} />
    </main>
  );
};

export default AnswerPage;
