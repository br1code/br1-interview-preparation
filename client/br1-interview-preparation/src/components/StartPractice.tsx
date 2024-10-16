'use client';

import { FC, useState } from 'react';
import { DropdownOption } from '@/types';
import { useRouter } from 'next/navigation';

interface StartPracticeProps {
  categoriesOptions: DropdownOption[];
}

const ALL_CATEGORIES_OPTION: DropdownOption = {
  label: 'All Categories',
  value: '',
};

const StartPractice: FC<StartPracticeProps> = ({ categoriesOptions }) => {
  const [selectedCategory, setSelectedCategory] =
    useState<DropdownOption | null>(ALL_CATEGORIES_OPTION);

  const router = useRouter();

  const onSelectCategory = (e: React.ChangeEvent<HTMLSelectElement>) => {
    const selectedOption =
      categoriesOptions.find((option) => option.value === e.target.value) ||
      ALL_CATEGORIES_OPTION;
    setSelectedCategory(selectedOption);
  };

  const onStartPractice = () => {
    const query = selectedCategory?.value
      ? `?categoryId=${selectedCategory.value}`
      : '';
    router.push(`/practice${query}`);
  };

  return (
    <section>
      <select value={selectedCategory?.value} onChange={onSelectCategory}>
        <option value="">All Categories</option>
        {categoriesOptions.map((category) => (
          <option key={category.value} value={category.value}>
            {category.label}
          </option>
        ))}
      </select>
      <button onClick={onStartPractice}>Start Practice</button>
    </section>
  );
};

export default StartPractice;
