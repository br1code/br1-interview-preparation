import Link from 'next/link';

const Navbar: React.FC = () => {
  return (
    <nav className="sticky top-0 bg-blue-600 shadow-md p-4 z-50 h-16">
      <div className="flex items-center justify-between max-w-7xl mx-auto">
        <Link href="/" className="text-xl font-bold text-white">
          Interview Preparation
        </Link>
        <div className="space-x-4">
          <Link href="/categories" className="text-white hover:text-gray-200">
            Categories
          </Link>
          <Link href="/questions" className="text-white hover:text-gray-200">
            Questions
          </Link>
        </div>
      </div>
    </nav>
  );
};

export default Navbar;
