import { FC } from 'react';

interface PaginationProps {
  pageNumber: number;
  pageSize: number;
  onNext: () => void;
  onPrevious: () => void;
  onPageSizeChange: (event: React.ChangeEvent<HTMLSelectElement>) => void;
  isNextDisabled: boolean;
}

// TODO: move to a different place, close to my "table" component
const PaginationControls: FC<PaginationProps> = ({
  pageNumber,
  pageSize,
  onNext,
  onPrevious,
  onPageSizeChange,
  isNextDisabled,
}) => {
  return (
    <div className="flex justify-between items-center mt-4">
      <button
        onClick={onPrevious}
        disabled={pageNumber === 1}
        className="bg-gray-300 text-gray-700 px-4 py-2 rounded-md disabled:opacity-50 disabled:cursor-not-allowed"
      >
        Previous
      </button>

      <span>Page {pageNumber}</span>

      <div className="flex items-center gap-2">
        <label className="mr-2">Page Size:</label>
        <select
          value={pageSize}
          onChange={onPageSizeChange}
          className="border border-gray-300 rounded-md px-4 py-2"
        >
          <option value={5}>5</option>
          <option value={10}>10</option>
          <option value={20}>20</option>
          <option value={50}>50</option>
        </select>
        <button
          onClick={onNext}
          disabled={isNextDisabled}
          className="bg-gray-300 text-gray-700 px-4 py-2 rounded-md disabled:opacity-50 disabled:cursor-not-allowed"
        >
          Next
        </button>
      </div>
    </div>
  );
};

export default PaginationControls;
