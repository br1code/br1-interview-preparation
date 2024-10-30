'use client';

import { FC, useState } from 'react';
import Link from 'next/link';
import { DropdownOption } from '@/types';
import { useRouter } from 'next/navigation';
import CategoryDropdown from './categories/CategoryDropdown';
import useFetchCategories from '@/hooks/useFetchCategories';
import { toDropdownOptions } from '@/utils';

const ALL_CATEGORIES_OPTION: DropdownOption = {
  label: 'All Categories',
  value: '',
};

const StartPractice: FC = () => {
  const [selectedCategory, setSelectedCategory] =
    useState<DropdownOption | null>(ALL_CATEGORIES_OPTION);

  const router = useRouter();

  const {
    categories,
    loading: categoriesLoading,
    error: categoriesError,
  } = useFetchCategories();

  const onStartPractice = () => {
    const query = selectedCategory?.value
      ? `?categoryId=${selectedCategory.value}`
      : '';
    router.push(`/practice${query}`);
  };

  return (
    <section className="flex flex-col items-center">
      {categoriesError ? (
        <p className="text-red-500 mb-4">
          Error loading categories: {categoriesError}
        </p>
      ) : (
        <CategoryDropdown
          categories={toDropdownOptions(categories)}
          selectedCategory={selectedCategory}
          onSelectCategory={setSelectedCategory}
          includeAllOption={true}
          loading={categoriesLoading}
          className="mb-6 w-full"
        />
      )}

      <button
        onClick={onStartPractice}
        disabled={categoriesLoading || !!categoriesError}
        className={`bg-blue-600 text-white px-6 py-3 rounded-md transition ${
          categoriesLoading || categoriesError
            ? 'opacity-50 cursor-not-allowed'
            : 'hover:bg-blue-700'
        }`}
      >
        Start Practice
      </button>

      <Link href="/questions">
        <button className="bg-indigo-600 text-white px-6 py-3 rounded-md hover:bg-indigo-700 transition mt-4">
          Edit Questions
        </button>
      </Link>
    </section>
  );
};

export default StartPractice;
