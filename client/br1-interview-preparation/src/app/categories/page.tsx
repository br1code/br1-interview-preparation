import { FC } from 'react';
import Link from 'next/link';
import CategoriesList from '@/components/categories/CategoriesList';

const CategoriesPage: FC = () => {
  return (
    <main className="flex flex-col items-center p-8">
      <h1 className="text-3xl font-bold mb-6">Categories</h1>
      <Link href="/categories/add">
        <button className="bg-green-600 text-white px-6 py-3 rounded-md hover:bg-green-700 transition mb-6">
          Add Category
        </button>
      </Link>

      <CategoriesList />
    </main>
  );
};

export default CategoriesPage;
