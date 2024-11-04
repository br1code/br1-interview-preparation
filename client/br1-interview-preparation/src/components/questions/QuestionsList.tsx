'use client';

import { FC, useState } from 'react';
import Link from 'next/link';
import { deleteQuestion } from '@/api';
import { DropdownOption } from '@/types';
import useFetchQuestions from '@/hooks/useFetchQuestions';
import CategoryDropdown from '../categories/CategoryDropdown';
import useFetchCategories from '@/hooks/useFetchCategories';
import { toDropdownOptions } from '@/utils';
import PaginationControls from '../PaginationControls';

const DEFAULT_PAGE_NUMBER = 1;
const DEFAULT_PAGE_SIZE = 10;

// TODO: split into different components
const QuestionsList: FC = () => {
  const [selectedCategory, setSelectedCategory] =
    useState<DropdownOption | null>(null);
  const [searchCategoryId, setSearchCategoryId] = useState<string | null>(null);
  const [searchContentInput, setSearchContentInput] = useState<string>('');
  const [searchContent, setSearchContent] = useState<string>('');
  const [pageNumber, setPageNumber] = useState<number>(DEFAULT_PAGE_NUMBER);
  const [pageSize, setPageSize] = useState<number>(DEFAULT_PAGE_SIZE);
  const [refreshKey, setRefreshKey] = useState<number>(0);

  const {
    questions,
    loading: questionsLoading,
    error: questionsError,
  } = useFetchQuestions(
    {
      categoryId: searchCategoryId,
      content: searchContent,
      pageNumber,
      pageSize,
    },
    refreshKey
  );

  const sortedQuestions = questions
    ? questions.slice().sort((a, b) => b.answersCount - a.answersCount)
    : [];

  const {
    categories,
    loading: categoriesLoading,
    error: categoriesError,
  } = useFetchCategories();

  const onClickSearch = () => {
    setSearchCategoryId(selectedCategory?.value || null);
    setSearchContent(searchContentInput);
    setPageNumber(1);
  };

  // TODO: stop using `confirm`, create reusable modal component
  const handleDelete = async (questionId: string) => {
    if (
      confirm('Are you sure you want to delete this question and its answers?')
    ) {
      await deleteQuestion(questionId);
      setRefreshKey((prevKey) => prevKey + 1);
    }
  };

  const getCategoryName = (categoryId: string) => {
    return (
      categories?.find((category) => category.id === categoryId)?.name ||
      'Unknown'
    );
  };

  const handleNextPage = () => {
    setPageNumber((prevPage) => prevPage + 1);
  };

  const handlePreviousPage = () => {
    setPageNumber((prevPage) => Math.max(1, prevPage - 1));
  };

  const handlePageSizeChange = (
    event: React.ChangeEvent<HTMLSelectElement>
  ) => {
    setPageSize(Number(event.target.value));
    setPageNumber(1);
  };

  return (
    <div className="w-full max-w-6xl">
      {/* Category Dropdown, Content Search, Page Size, and Search Button */}
      <div className="flex items-center justify-between mb-6 gap-4">
        {categoriesError ? (
          <p className="text-red-500 mb-4">
            Error loading categories: {categoriesError}
          </p>
        ) : (
          <CategoryDropdown
            categories={toDropdownOptions(categories)}
            selectedCategory={selectedCategory}
            onSelectCategory={setSelectedCategory}
            loading={categoriesLoading}
            includeAllOption={true}
            className="w-full max-w-xs"
          />
        )}
        <input
          type="text"
          value={searchContentInput}
          onChange={(e) => setSearchContentInput(e.target.value)}
          placeholder="Search by content"
          className="border border-gray-300 rounded-md px-4 py-2 w-full max-w-2xl"
        />
        <button
          onClick={onClickSearch}
          disabled={questionsLoading || categoriesLoading}
          className={`bg-blue-600 text-white px-6 py-2 rounded-md transition ${
            questionsLoading
              ? 'opacity-50 cursor-not-allowed'
              : 'hover:bg-blue-700'
          }`}
        >
          Search
        </button>
      </div>

      {/* Questions Table or Loading Spinner */}
      {questionsLoading ? (
        <div className="flex justify-center items-center py-10">
          <div className="loader border-t-4 border-blue-600 rounded-full w-8 h-8 animate-spin"></div>
        </div>
      ) : questionsError || categoriesError ? (
        <p className="text-red-500 text-center">
          Error loading questions: {questionsError || categoriesError}
        </p>
      ) : sortedQuestions.length > 0 ? (
        <>
          <table className="w-full border-collapse border border-gray-200">
            <thead>
              <tr>
                <th className="border border-gray-200 p-3 text-left">
                  Content
                </th>
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
              {sortedQuestions.map((question) => (
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

          {/* Pagination Controls */}
          <PaginationControls
            pageNumber={pageNumber}
            pageSize={pageSize}
            onNext={handleNextPage}
            onPrevious={handlePreviousPage}
            onPageSizeChange={handlePageSizeChange}
            isNextDisabled={sortedQuestions.length < pageSize}
          />
        </>
      ) : (
        <>
          <p className="text-center mt-4">No questions found ...</p>
          <PaginationControls
            pageNumber={pageNumber}
            pageSize={pageSize}
            onNext={handleNextPage}
            onPrevious={handlePreviousPage}
            onPageSizeChange={handlePageSizeChange}
            isNextDisabled
          />
        </>
      )}
    </div>
  );
};

export default QuestionsList;
