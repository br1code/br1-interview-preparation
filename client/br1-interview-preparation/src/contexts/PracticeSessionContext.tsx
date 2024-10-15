import {
  createContext,
  FC,
  ReactNode,
  useCallback,
  useContext,
  useEffect,
  useMemo,
  useReducer,
} from 'react';
import { Question } from '@/types';
import { fetchRandomQuestion } from '@/api';

interface PracticeSessionState {
  currentQuestion: Question | null;
  loadingQuestion: boolean;
  showHint: boolean;
  isRecording: boolean;
  categoryId?: string | null;
  error: string | null;
}

type PracticeSessionAction =
  | { type: 'SET_CURRENT_QUESTION'; payload: Question | null }
  | { type: 'SET_LOADING_QUESTION'; payload: boolean }
  | { type: 'SET_SHOW_HINT'; payload: boolean }
  | { type: 'SET_CATEGORY_ID'; payload: string | null }
  | { type: 'START_RECORDING' }
  | { type: 'STOP_RECORDING' }
  | { type: 'SET_ERROR'; payload: string | null };

interface PracticeSessionContextProps {
  state: PracticeSessionState;
  setCurrentQuestion: (question: Question | null) => void;
  setLoadingQuestion: (isLoading: boolean) => void;
  setShowHint: (show: boolean) => void;
  setCategoryId: (categoryId: string | null) => void;
  startRecording: () => void;
  stopRecording: () => void;
  setError: (errorMessage: string | null) => void;
  fetchNextQuestion: () => void;
}

const initialState: PracticeSessionState = {
  currentQuestion: null,
  loadingQuestion: false,
  isRecording: false,
  categoryId: undefined,
  error: null,
  showHint: false,
};

const reducer = (
  state: PracticeSessionState,
  action: PracticeSessionAction
): PracticeSessionState => {
  switch (action.type) {
    case 'SET_CURRENT_QUESTION':
      return { ...state, currentQuestion: action.payload };
    case 'SET_LOADING_QUESTION':
      return { ...state, loadingQuestion: action.payload };
    case 'SET_SHOW_HINT':
      return { ...state, showHint: action.payload };
    case 'SET_CATEGORY_ID':
      return { ...state, categoryId: action.payload };
    case 'START_RECORDING':
      return { ...state, isRecording: true };
    case 'STOP_RECORDING':
      return { ...state, isRecording: false };
    case 'SET_ERROR':
      return { ...state, error: action.payload };
    default:
      return state;
  }
};

const PracticeSessionContext = createContext<
  PracticeSessionContextProps | undefined
>(undefined);

interface PracticeSessionProviderProps {
  children?: ReactNode | undefined;
}

const PracticeSessionProvider: FC<PracticeSessionProviderProps> = ({
  children,
}) => {
  const [state, dispatch] = useReducer(reducer, initialState);

  const setCurrentQuestion = useCallback(
    (question: Question | null) => {
      dispatch({ type: 'SET_CURRENT_QUESTION', payload: question });
    },
    [dispatch]
  );

  const setLoadingQuestion = useCallback(
    (isLoading: boolean) => {
      dispatch({ type: 'SET_LOADING_QUESTION', payload: isLoading });
    },
    [dispatch]
  );

  const setShowHint = useCallback(
    (show: boolean) => {
      dispatch({ type: 'SET_SHOW_HINT', payload: show });
    },
    [dispatch]
  );

  const setCategoryId = useCallback(
    (categoryId: string | null) => {
      dispatch({ type: 'SET_CATEGORY_ID', payload: categoryId });
    },
    [dispatch]
  );

  const startRecording = useCallback(() => {
    dispatch({ type: 'START_RECORDING' });
  }, [dispatch]);

  const stopRecording = useCallback(() => {
    dispatch({ type: 'STOP_RECORDING' });
  }, [dispatch]);

  const setError = useCallback(
    (errorMessage: string | null) => {
      dispatch({ type: 'SET_ERROR', payload: errorMessage });
    },
    [dispatch]
  );

  const fetchNextQuestion = useCallback(async () => {
    try {
      setLoadingQuestion(true);
      setError(null);
      setShowHint(false);

      const question = await fetchRandomQuestion(state.categoryId);

      setCurrentQuestion(question);
      console.log('Question fetched', question);
    } catch (error) {
      console.error('Error fetching question:', error);
      setError(
        (error as Error)?.message ||
          'Failed to load question. Please try again.'
      );
    } finally {
      setLoadingQuestion(false);
    }
  }, [
    state.categoryId,
    setCurrentQuestion,
    setError,
    setLoadingQuestion,
    setShowHint,
  ]);

  // Fetch the initial question when categoryId changes
  useEffect(() => {
    if (state.categoryId !== undefined) {
      fetchNextQuestion();
    }
  }, [state.categoryId, fetchNextQuestion]);

  const contextValue: PracticeSessionContextProps = useMemo(
    () => ({
      state,
      setCurrentQuestion,
      setLoadingQuestion,
      setShowHint,
      setCategoryId,
      startRecording,
      stopRecording,
      setError,
      fetchNextQuestion,
    }),
    [
      state,
      setCurrentQuestion,
      setLoadingQuestion,
      setShowHint,
      setCategoryId,
      startRecording,
      stopRecording,
      setError,
      fetchNextQuestion,
    ]
  );

  return (
    <PracticeSessionContext.Provider value={contextValue}>
      {children}
    </PracticeSessionContext.Provider>
  );
};

const usePracticeSession = () => {
  const context = useContext(PracticeSessionContext);

  if (!context) {
    throw new Error(
      'Attempted to use PracticeSessionContext outside of its provider'
    );
  }

  return context;
};

export { PracticeSessionProvider, usePracticeSession };
