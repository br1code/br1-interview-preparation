import { FC, ChangeEvent } from 'react';
import { DropdownOption } from '@/types';

interface CategoryDropdownProps {
  categories: DropdownOption[];
  selectedCategory: DropdownOption | null;
  onSelectCategory: (category: DropdownOption) => void;
  includeAllOption?: boolean;
  loading?: boolean;
  className?: string;
}

const ALL_CATEGORIES_OPTION: DropdownOption = {
  label: 'All Categories',
  value: '',
};

const CategoryDropdown: FC<CategoryDropdownProps> = ({
  categories,
  selectedCategory,
  onSelectCategory,
  includeAllOption = true,
  loading = false,
  className = '',
}) => {
  const handleSelectChange = (e: ChangeEvent<HTMLSelectElement>) => {
    const selectedOption =
      categories.find((category) => category.value === e.target.value) ||
      ALL_CATEGORIES_OPTION;
    onSelectCategory(selectedOption);
  };

  return (
    <select
      id="categoryId"
      value={selectedCategory?.value || ''}
      onChange={handleSelectChange}
      disabled={loading}
      className={`px-4 py-2 border border-gray-300 rounded-md ${className}`}
    >
      {loading ? (
        <option>Loading categories...</option>
      ) : (
        <>
          {!includeAllOption && (
            <option value="" disabled>
              Select Category
            </option>
          )}
          {includeAllOption && (
            <option value="">{ALL_CATEGORIES_OPTION.label}</option>
          )}
          {categories.map((category) => (
            <option key={category.value} value={category.value}>
              {category.label}
            </option>
          ))}
        </>
      )}
    </select>
  );
};

export default CategoryDropdown;
