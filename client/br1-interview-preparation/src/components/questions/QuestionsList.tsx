'use client';

import { FC, useState } from 'react';
import Link from 'next/link';
import { deleteQuestion } from '@/api';
import { DropdownOption } from '@/types';
import useFetchQuestions from '@/hooks/useFetchQuestions';

interface QuestionsListProps {
  categoriesOptions: DropdownOption[];
}

const ALL_CATEGORIES_OPTION: DropdownOption = {
  label: 'All Categories',
  value: '',
};

// TODO: try to split into multiple components
const QuestionsList: FC<QuestionsListProps> = ({ categoriesOptions }) => {
  const [selectedCategory, setSelectedCategory] = useState<DropdownOption>(
    ALL_CATEGORIES_OPTION
  );
  const [searchCategoryId, setSearchCategoryId] = useState<string | null>(null);
  const [refreshKey, setRefreshKey] = useState(0);

  const { questions, loading, error } = useFetchQuestions(
    searchCategoryId,
    refreshKey
  );

  const onSelectCategory = (e: React.ChangeEvent<HTMLSelectElement>) => {
    const selectedOption =
      categoriesOptions.find((option) => option.value === e.target.value) ||
      ALL_CATEGORIES_OPTION;
    setSelectedCategory(selectedOption);
  };

  const onClickSearch = () => {
    setSearchCategoryId(selectedCategory?.value || null);
  };

  // TODO: stop using `confirm`
  const handleDelete = async (questionId: string) => {
    if (confirm('Are you sure you want to delete this question?')) {
      await deleteQuestion(questionId);
      setRefreshKey((prevKey) => prevKey + 1);
    }
  };

  const getCategoryName = (categoryId: string) => {
    return (
      categoriesOptions.find((category) => category.value === categoryId)
        ?.label || 'Unknown'
    );
  };

  return (
    <div className="w-full max-w-3xl">
      {/* Category Dropdown and Search Button */}
      <div className="flex items-center justify-center mb-6 gap-4">
        <select
          value={selectedCategory?.value || ''}
          onChange={onSelectCategory}
          disabled={loading}
          className="px-4 py-2 border border-gray-300 rounded-md w-full max-w-xs"
        >
          <option value="">All Categories</option>
          {categoriesOptions.map((category) => (
            <option key={category.value} value={category.value}>
              {category.label}
            </option>
          ))}
        </select>
        <button
          onClick={onClickSearch}
          disabled={loading}
          className={`bg-blue-600 text-white px-6 py-2 rounded-md transition ${
            loading ? 'opacity-50 cursor-not-allowed' : 'hover:bg-blue-700'
          }`}
        >
          Search
        </button>
      </div>

      {/* Questions Table or Loading Spinner */}
      {loading ? (
        <div className="flex justify-center items-center py-10">
          <div className="loader border-t-4 border-blue-600 rounded-full w-8 h-8 animate-spin"></div>
        </div>
      ) : error || !questions ? (
        <p className="text-red-500 text-center">
          Error loading questions: {error}
        </p>
      ) : questions.length > 0 ? (
        <table className="w-full border-collapse border border-gray-200">
          <thead>
            <tr>
              <th className="border border-gray-200 p-3 text-left">Content</th>
              <th className="border border-gray-200 p-3 text-center">
                Category
              </th>
              <th className="border border-gray-200 p-3 text-center">
                Answers Count
              </th>
              <th className="border border-gray-200 p-3 text-center">
                Actions
              </th>
            </tr>
          </thead>
          <tbody>
            {questions.map((question) => (
              <tr key={question.id} className="text-center">
                <td className="border border-gray-200 p-3 text-left">
                  {question.content}
                </td>
                <td className="border border-gray-200 p-3">
                  {getCategoryName(question.categoryId)}
                </td>
                <td className="border border-gray-200 p-3">
                  {question.answersCount}
                </td>
                <td className="border border-gray-200 p-3 flex justify-center gap-4">
                  <Link
                    href={`/questions/${question.id}`}
                    target="_blank"
                    rel="noopener noreferrer"
                  >
                    <span className="text-blue-600 cursor-pointer hover:text-blue-800">
                      ✏️
                    </span>
                  </Link>
                  <span
                    onClick={() => handleDelete(question.id)}
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
        <p className="text-center mt-4">No questions yet ...</p>
      )}
    </div>
  );
};

export default QuestionsList;
