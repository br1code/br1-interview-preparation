'use client';

import { FC } from 'react';
import useFetchAnswer from '@/hooks/useFetchAnswer';
import Link from 'next/link';
import { deleteAnswer } from '@/api';
import { useRouter } from 'next/navigation';

interface AnswerDetailsProps {
  answerId: string;
}

const API_URL = process.env.NEXT_PUBLIC_API_URL;

const AnswerDetails: FC<AnswerDetailsProps> = ({ answerId }) => {
  const router = useRouter();

  const {
    answer,
    loading: loadingAnswer,
    error: answerError,
  } = useFetchAnswer(answerId);

  if (loadingAnswer) {
    return <p>Loading answer...</p>;
  }

  if (answerError || !answer) {
    return <p className="text-red-500">Error loading answer: {answerError}</p>;
  }

  // TODO: stop using `confirm`
  const handleDeleteAnswer = async (answerId: string) => {
    if (confirm('Are you sure you want to delete this answer?')) {
      await deleteAnswer(answerId);
      alert('Answer deleted successfully');
      router.push(`/questions/${answer.questionId}`);
    }
  };

  return (
    <main className="flex min-h-screen items-center justify-center bg-gray-100 p-4">
      <div className="w-full max-w-2xl bg-white p-8 shadow-md rounded-lg text-center">
        <h1 className="text-3xl font-bold mb-6">Answer Details</h1>
        <p className="text-lg mb-4">
          {new Date(answer.createdAt).toLocaleString()}
        </p>

        <video controls className="w-full border-2 border-gray-300 rounded-md">
          <source src={`${API_URL}/answers/${answerId}`} type="video/mp4" />
          Your browser does not support the video tag.
        </video>

        <div className="mt-8">
          <button
            onClick={() => handleDeleteAnswer(answer.id)}
            className="bg-red-600 text-white px-4 py-2 rounded-md hover:bg-red-700 transition"
          >
            Delete Answer
          </button>
        </div>

        <div className="mt-8">
          <Link
            href={`/questions/${answer.questionId}`}
            className="text-blue-600 underline hover:text-blue-800"
          >
            Return to Question
          </Link>
        </div>
      </div>
    </main>
  );
};

export default AnswerDetails;
