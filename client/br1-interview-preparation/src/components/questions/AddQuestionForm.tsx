'use client';

import { FC, useState, useEffect } from 'react';
import Link from 'next/link';
import { SubmitHandler, useForm } from 'react-hook-form';
import { DropdownOption } from '@/types';
import { addQuestion } from '@/api';
import CategoryDropdown from '../categories/CategoryDropdown';
import useFetchCategories from '@/hooks/useFetchCategories';
import { toDropdownOptions } from '@/utils';

interface QuestionFormValues {
  content: string;
  hint: string;
  categoryId: string;
}

const AddQuestionForm: FC = () => {
  const [lastAddedQuestionId, setLastAddedQuestionId] = useState<string | null>(
    null
  );

  const [selectedCategory, setSelectedCategory] =
    useState<DropdownOption | null>(null);

  const {
    register,
    handleSubmit,
    formState: { errors },
    reset,
    setValue,
  } = useForm<QuestionFormValues>({
    defaultValues: {
      content: '',
      hint: '',
      categoryId: '',
    },
  });

  const {
    categories,
    loading: categoriesLoading,
    error: categoriesError,
  } = useFetchCategories();

  useEffect(() => {
    register('categoryId', { required: 'Category is required' });
  }, [register]);

  const onSubmit: SubmitHandler<QuestionFormValues> = async (data) => {
    try {
      const result = await addQuestion(data);
      setLastAddedQuestionId(result);
      alert('Question created successfully.');
      reset({
        content: '',
        hint: '',
        categoryId: selectedCategory?.value,
      });
    } catch (error) {
      console.log(error);
    }
  };

  return (
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
        {categoriesError ? (
          <p className="text-red-500 mb-4">
            Error loading categories: {categoriesError}
          </p>
        ) : (
          <>
            <CategoryDropdown
              categories={toDropdownOptions(categories)}
              selectedCategory={selectedCategory}
              onSelectCategory={(selectedOption: DropdownOption) => {
                setValue('categoryId', selectedOption.value);
                setSelectedCategory(selectedOption);
              }}
              loading={categoriesLoading}
              includeAllOption={false}
              className="w-full"
            />
            {errors.categoryId && (
              <p className="text-red-500 mb-2">{errors.categoryId.message}</p>
            )}
          </>
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
  );
};

export default AddQuestionForm;
