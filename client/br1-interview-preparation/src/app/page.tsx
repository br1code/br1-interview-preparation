import StartPractice from '@/components/StartPractice';
import { fetchCategories } from '@/http';
import { toDropdownOptions } from '@/utils';

export default async function Home() {
  const categories = await fetchCategories();

  return (
    <main>
      <h1>Interview Preparation</h1>
      <StartPractice categoriesOptions={toDropdownOptions(categories)} />
    </main>
  );
}
