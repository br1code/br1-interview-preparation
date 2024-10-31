'use client';

import { FC, useEffect, useState } from 'react';
import { useRouter } from 'next/navigation';
import Link from 'next/link';
import { useForm, SubmitHandler } from 'react-hook-form';
import CategoryDropdown from '../categories/CategoryDropdown';
import useFetchCategories from '@/hooks/useFetchCategories';
import useFetchQuestion from '@/hooks/useFetchQuestion';
import { DropdownOption, Answer } from '@/types';
import { updateQuestion, deleteQuestion } from '@/api';
import { toDropdownOption, toDropdownOptions } from '@/utils';

interface QuestionDetailsProps {
  questionId: string;
}

interface QuestionFormValues {
  content: string;
  hint: string;
  categoryId: string;
}

// TODO: try to split into multiple components
const QuestionDetails: FC<QuestionDetailsProps> = ({ questionId }) => {
  const router = useRouter();

  const {
    question,
    loading: questionLoading,
    error: questionError,
  } = useFetchQuestion(questionId);

  const {
    categories,
    loading: categoriesLoading,
    error: categoriesError,
  } = useFetchCategories();

  const {
    register,
    handleSubmit,
    setValue,
    formState: { errors },
  } = useForm<QuestionFormValues>();
  const [answers, setAnswers] = useState<Answer[]>([]);
  const [selectedCategory, setSelectedCategory] =
    useState<DropdownOption | null>(null);

  useEffect(() => {
    if (question && categories && categories.length) {
      setValue('content', question.content);
      setValue('hint', question.hint || '');
      setValue('categoryId', question.categoryId);
      setAnswers(question.answers || []);

      const initialCategory = categories.find(
        (category) => category.id === question.categoryId
      );

      if (initialCategory) {
        setSelectedCategory(toDropdownOption(initialCategory));
      }
    }
  }, [question, categories, setValue]);

  const onSubmit: SubmitHandler<QuestionFormValues> = async (data) => {
    try {
      await updateQuestion(questionId, data);
      alert('Question updated successfully');
    } catch (error) {
      console.log(error);
      alert('Failed to update question.');
    }
  };

  // TODO: stop using `confirm`
  const handleDeleteQuestion = async () => {
    if (confirm('Are you sure you want to delete this question?')) {
      await deleteQuestion(questionId);
      alert('Question deleted successfully');
      router.push('/questions');
    }
  };

  if (questionLoading) {
    return (
      <div className="flex justify-center items-center py-10">
        <div className="loader border-t-4 border-blue-600 rounded-full w-8 h-8 animate-spin"></div>
      </div>
    );
  }

  if (questionError) {
    return (
      <p className="text-red-500">Error loading question: {questionError}</p>
    );
  }

  if (categoriesError) {
    return (
      <p className="text-red-500">
        Error loading categories: {categoriesError}
      </p>
    );
  }

  return (
    <div className="w-full max-w-3xl bg-white p-8 shadow-md rounded-lg">
      <h1 className="text-3xl font-bold mb-6 text-center">Question Details</h1>
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
            htmlFor="categoryId"
            className="block text-lg font-semibold mb-2"
          >
            Category
          </label>
          <CategoryDropdown
            categories={toDropdownOptions(categories)}
            selectedCategory={selectedCategory}
            onSelectCategory={(selectedOption: DropdownOption) => {
              setValue('categoryId', selectedOption.value);
              setSelectedCategory(selectedOption);
            }}
            includeAllOption={false}
            loading={categoriesLoading}
            className="w-full"
          />
          {errors.categoryId && (
            <p className="text-red-500 mb-2">{errors.categoryId.message}</p>
          )}
        </div>

        <div className="text-center">
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
        </div>
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
              >
                {new Date(answer.createdAt).toLocaleString()}
              </Link>
            </li>
          ))}
        </ul>
      ) : (
        <p className="mb-6">No answers submitted yet.</p>
      )}
    </div>
  );
};

export default QuestionDetails;
