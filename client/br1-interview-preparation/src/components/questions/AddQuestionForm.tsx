'use client';

import { FC, useState } from 'react';
import Link from 'next/link';
import { SubmitHandler, useForm } from 'react-hook-form';
import { DropdownOption } from '@/types';
import { addQuestion } from '@/api';

interface AddQuestionFormProps {
  categoriesOptions: DropdownOption[];
}

interface QuestionFormValues {
  content: string;
  hint: string;
  categoryId: string;
}

const AddQuestionForm: FC<AddQuestionFormProps> = ({ categoriesOptions }) => {
  const [lastAddedQuestionId, setLastAddedQuestionId] = useState<string | null>(
    null
  );

  const {
    register,
    handleSubmit,
    formState: { errors },
    reset,
  } = useForm<QuestionFormValues>({
    defaultValues: {
      content: '',
      hint: '',
      categoryId: '',
    },
  });

  const onSubmit: SubmitHandler<QuestionFormValues> = async (data) => {
    try {
      const result = await addQuestion(data);
      setLastAddedQuestionId(result);
      alert('Question created successfully.');
      reset();
    } catch (error) {
      console.log(error);
    }
  };

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
            <option value="" disabled>
              Select Category
            </option>
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

        {/* Add Question Button */}
        <div className="text-center">
          <button
            type="submit"
            className="bg-blue-600 text-white px-6 py-3 rounded-md hover:bg-blue-700 transition mb-6"
          >
            Add Question
          </button>
        </div>

        {/* Link to edit the last added question */}
        <div className="text-center">
          {lastAddedQuestionId && (
            <Link
              href={`/questions/${lastAddedQuestionId}`}
              target="_blank"
              rel="noopener noreferrer"
            >
              <button
                type="button"
                className="bg-green-600 text-white px-6 py-3 rounded-md hover:bg-green-700 transition mb-4"
              >
                Edit Last Question Added
              </button>
            </Link>
          )}
        </div>
      </form>

      {/* Return to Homepage */}
      <div className="text-center">
        <Link href="/" className="text-blue-600 underline hover:text-blue-800">
          Return to Homepage
        </Link>
      </div>
    </div>
  );
};

export default AddQuestionForm;
