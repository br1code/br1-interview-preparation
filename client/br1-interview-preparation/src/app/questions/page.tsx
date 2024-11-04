import Link from 'next/link';
import QuestionsList from '@/components/questions/QuestionsList';

export default function QuestionsPage() {
  return (
    <main className="flex flex-col items-center p-4">
      <h1 className="text-3xl font-bold mb-4">Questions</h1>
      <Link href="/questions/add">
        <button className="bg-green-600 text-white px-6 py-3 rounded-md hover:bg-green-700 transition mb-6">
          Add Question
        </button>
      </Link>

      <QuestionsList />
    </main>
  );
}
