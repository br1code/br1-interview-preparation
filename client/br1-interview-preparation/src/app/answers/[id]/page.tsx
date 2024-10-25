import { FC } from 'react';
import AnswerDetails from '@/components/answers/AnswerDetails';

interface AnswerProps {
  params: {
    id: string;
  };
}

const Answer: FC<AnswerProps> = ({ params }) => {
  const { id } = params;

  return <AnswerDetails answerId={id} />;
};

export default Answer;
