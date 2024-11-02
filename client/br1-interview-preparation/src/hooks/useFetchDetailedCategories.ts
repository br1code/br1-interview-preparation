import { useState, useEffect } from 'react';
import { CategoryDetails } from '@/types';
import { fetchDetailedCategories } from '@/api';

interface UseFetchDetailedCategoriesResult {
  categories: CategoryDetails[] | null;
  loading: boolean;
  error: string | null;
}

const useFetchDetailedCategories = (
  refreshKey?: number
): UseFetchDetailedCategoriesResult => {
  const [categories, setCategories] = useState<CategoryDetails[] | null>(null);
  const [loading, setLoading] = useState<boolean>(true); // initialized to true on purpose
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchAndSetCategories = async () => {
      try {
        setLoading(true);
        setError(null);
        const fetchedCategories = await fetchDetailedCategories();
        setCategories(fetchedCategories);
      } catch (error) {
        console.error('Error fetching categories:', error);
        setError('Failed to load categories.');
        setCategories(null);
      } finally {
        setLoading(false);
      }
    };

    fetchAndSetCategories();
  }, [refreshKey]);

  return { categories, loading, error };
};

export default useFetchDetailedCategories;
