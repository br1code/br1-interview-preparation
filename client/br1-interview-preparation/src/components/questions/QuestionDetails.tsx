'use client';

import { FC, useEffect, useState } from 'react';
import { useRouter } from 'next/navigation';
import Link from 'next/link';
import { useForm, SubmitHandler } from 'react-hook-form';
import { DropdownOption, Answer } from '@/types';
import useFetchQuestion from '@/hooks/useFetchQuestion';
import { updateQuestion, deleteQuestion } from '@/api';

interface QuestionDetailsProps {
  questionId: string;
  categoriesOptions: DropdownOption[];
}

interface FormValues {
  content: string;
  hint: string;
  categoryId: string;
}

// TODO: try to split into multiple components
const QuestionDetails: FC<QuestionDetailsProps> = ({
  questionId,
  categoriesOptions,
}) => {
  const router = useRouter();

  const { question, loading, error } = useFetchQuestion(questionId);
  const {
    register,
    handleSubmit,
    setValue,
    formState: { errors },
  } = useForm<FormValues>();
  const [answers, setAnswers] = useState<Answer[]>([]);

  useEffect(() => {
    if (question) {
      setValue('content', question.content);
      setValue('hint', question.hint || '');
      setValue('categoryId', question.categoryId);
      setAnswers(question.answers || []);
    }
  }, [question, setValue]);

  const onSubmit: SubmitHandler<FormValues> = async (data) => {
    await updateQuestion(questionId, data);
    alert('Question updated successfully');
  };

  // TODO: stop using `confirm`
  const handleDeleteQuestion = async () => {
    if (confirm('Are you sure you want to delete this question?')) {
      await deleteQuestion(questionId);
      alert('Question deleted successfully');
      router.push('/');
    }
  };

  if (loading) {
    return <p>Loading question...</p>;
  }

  if (error) {
    return <p className="text-red-500">Error loading question: {error}</p>;
  }

  return (
    <div>
      <form onSubmit={handleSubmit(onSubmit)}>
        {/* Question Content and Hint */}
        <div className="mb-6">
          <label htmlFor="content" className="block text-lg font-semibold mb-2">
            Content
          </label>
          <textarea
            id="content"
            {...register('content', { required: 'Content is required' })}
            className="w-full p-2 border border-gray-300 rounded-md mb-4"
          />
          {errors.content && (
            <p className="text-red-500 mb-2">{errors.content.message}</p>
          )}

          <label htmlFor="hint" className="block text-lg font-semibold mb-2">
            Hint
          </label>
          <textarea
            id="hint"
            {...register('hint')}
            className="w-full p-2 border border-gray-300 rounded-md mb-4"
          />
        </div>

        {/* Category Selection Dropdown */}
        <div className="mb-6">
          <label
            htmlFor="category"
            className="block text-lg font-semibold mb-2"
          >
            Category
          </label>
          <select
            id="category"
            {...register('categoryId', { required: 'Category is required' })}
            className="w-full p-2 border border-gray-300 rounded-md"
          >
            {categoriesOptions.map((option) => (
              <option key={option.value} value={option.value}>
                {option.label}
              </option>
            ))}
          </select>
          {errors.categoryId && (
            <p className="text-red-500 mb-2">{errors.categoryId.message}</p>
          )}
        </div>

        {/* Update Question Button */}
        <button
          type="submit"
          className="bg-blue-600 text-white px-6 py-3 rounded-md hover:bg-blue-700 transition mb-8"
        >
          Update Question
        </button>

        {/* Delete Question Button*/}
        <button
          type="button"
          onClick={handleDeleteQuestion}
          className="bg-red-600 text-white px-6 py-3 rounded-md hover:bg-red-700 transition mb-8 ml-4"
        >
          Delete Question
        </button>
      </form>

      {/* Submitted Answers */}
      <h3 className="text-2xl font-semibold mb-4">Submitted Answers</h3>
      {answers.length > 0 ? (
        <ul className="mb-4">
          {answers.map((answer) => (
            <li key={answer.id} className="mb-4 list-disc">
              <Link
                href={`/answers/${answer.id}`}
                className="text-blue-600 underline mb-2 block"
                target="_blank"
              >
                {new Date(answer.createdAt).toLocaleString()}
              </Link>
            </li>
          ))}
        </ul>
      ) : (
        <p className="mb-6">No answers submitted yet.</p>
      )}

      {/* Return to Homepage */}
      <div className="text-center">
        <Link href="/" className="text-blue-600 underline hover:text-blue-800">
          Return to Homepage
        </Link>
      </div>
    </div>
  );
};

export default QuestionDetails;
