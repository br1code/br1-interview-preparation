import StartPractice from '@/components/StartPractice';
import { fetchCategories } from '@/api';
import { toDropdownOptions } from '@/utils';

export default async function Home() {
  const categories = await fetchCategories();

  return (
    <main className="flex min-h-screen items-center justify-center bg-gray-100 p-4">
      <div className="w-full max-w-md bg-white p-8 shadow-md rounded-lg text-center">
        <h1 className="text-3xl font-bold mb-6">Interview Preparation</h1>
        <StartPractice categoriesOptions={toDropdownOptions(categories)} />
      </div>
    </main>
  );
}
