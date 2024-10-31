import Link from 'next/link';
import QuestionsList from '@/components/questions/QuestionsList';

export default function QuestionsPage() {
  return (
    <main className="flex flex-col items-center p-8">
      <h1 className="text-3xl font-bold mb-6">Questions</h1>
      <Link href="/questions/add">
        <button className="bg-green-600 text-white px-6 py-3 rounded-md hover:bg-green-700 transition mb-6">
          Add Question
        </button>
      </Link>

      <QuestionsList />

      <div className="text-center mt-4">
        <Link href="/" className="text-blue-600 underline hover:text-blue-800">
          Return to Homepage
        </Link>
      </div>
    </main>
  );
}
