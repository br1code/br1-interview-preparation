import { useState, useEffect } from 'react';
import { Category } from '@/types';
import { fetchCategory } from '@/api';

interface UseFetchCategoryResult {
  category: Category | null;
  loading: boolean;
  error: string | null;
}

const useFetchCategory = (
  categoryId: string | null
): UseFetchCategoryResult => {
  const [category, setCategory] = useState<Category | null>(null);
  const [loading, setLoading] = useState<boolean>(true); // initialized to true on purpose
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchAndSetCategory = async () => {
      // 'All Categories'
      if (categoryId === null) {
        setCategory(null);
        setLoading(false);
        return;
      }

      try {
        setError(null);
        const fetchedCategory = await fetchCategory(categoryId);
        setCategory(fetchedCategory);
      } catch (error) {
        console.error('Error fetching category:', error);
        setError('Failed to load category.');
        setCategory(null);
      } finally {
        setLoading(false);
      }
    };

    fetchAndSetCategory();
  }, [categoryId]);

  return { category, loading, error };
};

export default useFetchCategory;
