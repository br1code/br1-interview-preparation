import { FC } from 'react';
import Link from 'next/link';
import { fetchQuestion } from '@/api';

interface QuestionProps {
  params: {
    id: string;
  };
}

const Question: FC<QuestionProps> = async ({ params }) => {
  const { id } = params;

  const question = await fetchQuestion(id);

  return (
    <main>
      <h1 className="text-4xl">{question.content}</h1>
      <h3 className="text-2xl">Answers</h3>
      <ul>
        {question.answers?.map((answer) => (
          <li key={answer.id}>
            <Link
              href={`/answers/${answer.id}`}
              className="text-blue-600 underline"
            >
              {answer.videoFilename}
            </Link>
          </li>
        ))}
      </ul>
    </main>
  );
};

export default Question;
