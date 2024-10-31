'use client';

import { FC, useEffect } from 'react';
import { SubmitHandler, useForm } from 'react-hook-form';
import { deleteCategory, updateCategory } from '@/api';
import useFetchCategory from '@/hooks/useFetchCategory';
import { useRouter } from 'next/navigation';

interface CategoryDetailsProps {
  categoryId: string;
}

interface CategoryFormValues {
  name: string;
}

const CategoryDetails: FC<CategoryDetailsProps> = ({ categoryId }) => {
  const router = useRouter();

  const { category, loading, error } = useFetchCategory(categoryId);

  const {
    register,
    handleSubmit,
    setValue,
    formState: { errors },
  } = useForm<CategoryFormValues>();

  useEffect(() => {
    if (category) {
      setValue('name', category.name);
    }
  }, [category, setValue]);

  const onSubmit: SubmitHandler<CategoryFormValues> = async (data) => {
    try {
      await updateCategory(categoryId, data);
      alert('Category updated successfully');
    } catch (error) {
      console.log(error);
      alert('Failed to update category.');
    }
  };

  // TODO: stop using `confirm`
  const handleDeleteCategory = async () => {
    if (
      confirm(
        'Are you sure you want to delete this category and its related questions and answers?'
      )
    ) {
      await deleteCategory(categoryId);
      alert('Category deleted successfully');
      router.push('/categories');
    }
  };

  if (loading) {
    return (
      <div className="flex justify-center items-center py-10">
        <div className="loader border-t-4 border-blue-600 rounded-full w-8 h-8 animate-spin"></div>
      </div>
    );
  }

  if (error) {
    return <p className="text-red-500">Error loading category: {error}</p>;
  }

  return (
    <div className="w-full max-w-3xl bg-white p-8 shadow-md rounded-lg">
      <h1 className="text-3xl font-bold mb-6 text-center">Category Details</h1>
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
            className="bg-blue-600 text-white px-6 py-3 rounded-md hover:bg-blue-700 transition mb-8"
          >
            Update Category
          </button>
          <button
            type="button"
            onClick={handleDeleteCategory}
            className="bg-red-600 text-white px-6 py-3 rounded-md hover:bg-red-700 transition mb-8 ml-4"
          >
            Delete Category
          </button>
        </div>
      </form>
    </div>
  );
};

export default CategoryDetails;
