import { FC } from 'react';
import CategoryDetails from '@/components/categories/CategoryDetails';

interface CategoryPageProps {
  params: {
    id: string;
  };
}

const CategoryPage: FC<CategoryPageProps> = ({ params }) => {
  const { id } = params;

  return (
    <main className="flex min-h-screen items-center justify-center bg-gray-100 p-4">
      <CategoryDetails categoryId={id} />
    </main>
  );
};

export default CategoryPage;
