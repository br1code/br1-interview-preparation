'use client';

import { FC } from 'react';
import Link from 'next/link';
import useFetchDetailedCategories from '@/hooks/useFetchDetailedCategories';
import { deleteCategory } from '@/api';

const CategoriesList: FC = () => {
  const { categories, loading, error } = useFetchDetailedCategories();

  // TODO: stop using `confirm`
  const handleDelete = async (categoryId: string) => {
    if (
      confirm(
        'Are you sure you want to delete this category and its related questions and answers?'
      )
    ) {
      await deleteCategory(categoryId);
    }
  };

  return (
    <div className="w-full max-w-3xl">
      {loading ? (
        <div className="flex justify-center items-center py-10">
          <div className="loader border-t-4 border-blue-600 rounded-full w-8 h-8 animate-spin"></div>
        </div>
      ) : error ? (
        <p className="text-red-500 text-center">
          Error loading categories: {error}
        </p>
      ) : categories && categories.length > 0 ? (
        <table className="w-full border-collapse border border-gray-200">
          <thead>
            <tr>
              <th className="border border-gray-200 p-3 text-left">Name</th>
              <th className="border border-gray-200 p-3 text-center">
                Questions Count
              </th>
              <th className="border border-gray-200 p-3 text-center">
                Actions
              </th>
            </tr>
          </thead>
          <tbody>
            {categories.map((category) => (
              <tr key={category.id} className="text-center">
                <td className="border border-gray-200 p-3 text-left">
                  {category.name}
                </td>
                <td className="border border-gray-200 p-3">
                  {category.questionsCount}
                </td>
                <td className="border border-gray-200 p-3 flex justify-center gap-4">
                  <Link
                    href={`/categories/${category.id}`}
                    target="_blank"
                    rel="noopener noreferrer"
                  >
                    <span className="text-blue-600 cursor-pointer hover:text-blue-800">
                      ✏️
                    </span>
                  </Link>
                  <span
                    onClick={() => handleDelete(category.id)}
                    className="text-red-600 cursor-pointer hover:text-red-800"
                  >
                    🗑️
                  </span>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      ) : (
        <p className="text-center mt-4">No categories yet ...</p>
      )}
    </div>
  );
};

export default CategoriesList;
