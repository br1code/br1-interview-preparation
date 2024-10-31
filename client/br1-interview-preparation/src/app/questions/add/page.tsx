import AddQuestionForm from '@/components/questions/AddQuestionForm';

export default function AddQuestionPage() {
  return (
    <main className="flex min-h-[calc(100vh-4rem)] items-center justify-center bg-gray-100 p-4">
      <div className="w-full max-w-3xl bg-white p-8 shadow-md rounded-lg">
        <h1 className="text-3xl font-bold mb-6 text-center">Add Question</h1>
        <AddQuestionForm />
      </div>
    </main>
  );
}
