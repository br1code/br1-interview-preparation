import { useState, useEffect } from 'react';
import { Category } from '@/types';
import { fetchCategories } from '@/api';

interface UseFetchCategoriesResult {
  categories: Category[] | null;
  loading: boolean;
  error: string | null;
}

// TODO: sort categories alphabetically (from server?)
const useFetchCategories = (): UseFetchCategoriesResult => {
  const [categories, setCategories] = useState<Category[] | null>(null);
  const [loading, setLoading] = useState<boolean>(true); // initialized to true on purpose
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchAndSetCategories = async () => {
      try {
        setLoading(true);
        setError(null);
        const fetchedCategories = await fetchCategories();
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
  }, []);

  return { categories, loading, error };
};

export default useFetchCategories;
