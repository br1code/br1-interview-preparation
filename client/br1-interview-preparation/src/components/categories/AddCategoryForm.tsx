'use client';

import { FC, useState } from 'react';
import Link from 'next/link';
import { addCategory } from '@/api';
import { SubmitHandler, useForm } from 'react-hook-form';

interface CategoryFormValues {
  name: string;
}

const AddCategoryForm: FC = () => {
  const [lastAddedCategoryId, setLastAddedCategoryId] = useState<string | null>(
    null
  );

  const {
    register,
    handleSubmit,
    formState: { errors },
    reset,
  } = useForm<CategoryFormValues>({
    defaultValues: {
      name: '',
    },
  });

  const onSubmit: SubmitHandler<CategoryFormValues> = async (data) => {
    try {
      const result = await addCategory(data);
      setLastAddedCategoryId(result);
      alert('Category created successfully');
      reset();
    } catch (error) {
      console.log(error);
      alert('Failed to create category.');
    }
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)}>
      <div className="mb-6">
        <label htmlFor="name" className="block text-lg font-semibold mb-2">
          Name
        </label>
        <input
          type="text"
          id="name"
          {...register('name', { required: 'Name is required' })}
          className="w-full p-2 border border-gray-300 rounded-md mb-4"
        />
        {errors.name && (
          <p className="text-red-500 mb-2">{errors.name.message}</p>
        )}
      </div>

      <div className="text-center">
        <button
          type="submit"
          className="bg-blue-600 text-white px-6 py-3 rounded-md hover:bg-blue-700 transition mb-6"
        >
          Add Category
        </button>
      </div>

      <div className="text-center">
        {lastAddedCategoryId && (
          <Link
            href={`/categories/${lastAddedCategoryId}`}
            target="_blank"
            rel="noopener noreferrer"
          >
            <button
              type="button"
              className="bg-green-600 text-white px-6 py-3 rounded-md hover:bg-green-700 transition mb-4"
            >
              Edit Last Category Added
            </button>
          </Link>
        )}
      </div>
    </form>
  );
};

export default AddCategoryForm;
