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
import { Category, Question } from '@/types';
import { fetchRandomQuestion } from '@/api';

interface PracticeSessionState {
  sessionStarted: boolean;
  currentQuestion: Question | null;
  loadingQuestion: boolean;
  showHint: boolean;
  isRecording: boolean;
  category?: Category | null;
  error: string | null;
}

type PracticeSessionAction =
  | { type: 'START_SESSION' }
  | { type: 'END_SESSION' }
  | { type: 'SET_CURRENT_QUESTION'; payload: Question | null }
  | { type: 'SET_LOADING_QUESTION'; payload: boolean }
  | { type: 'SET_SHOW_HINT'; payload: boolean }
  | { type: 'SET_CATEGORY'; payload: Category | null }
  | { type: 'START_RECORDING' }
  | { type: 'STOP_RECORDING' }
  | { type: 'SET_ERROR'; payload: string | null };

interface PracticeSessionContextProps {
  state: PracticeSessionState;
  startSession: () => void;
  endSession: () => void;
  setCurrentQuestion: (question: Question | null) => void;
  setLoadingQuestion: (isLoading: boolean) => void;
  setShowHint: (show: boolean) => void;
  setCategory: (category: Category | null) => void;
  startRecording: () => void;
  stopRecording: () => void;
  setError: (errorMessage: string | null) => void;
  fetchNextQuestion: () => void;
}

const initialState: PracticeSessionState = {
  sessionStarted: false,
  currentQuestion: null,
  loadingQuestion: false,
  isRecording: false,
  category: undefined,
  error: null,
  showHint: false,
};

const reducer = (
  state: PracticeSessionState,
  action: PracticeSessionAction
): PracticeSessionState => {
  switch (action.type) {
    case 'START_SESSION':
      return { ...state, sessionStarted: true, error: null, showHint: false };

    case 'END_SESSION':
      return {
        ...state,
        sessionStarted: false,
        currentQuestion: null,
        error: null,
        showHint: false,
      };

    case 'SET_CURRENT_QUESTION':
      return { ...state, currentQuestion: action.payload };

    case 'SET_LOADING_QUESTION':
      return { ...state, loadingQuestion: action.payload };

    case 'SET_SHOW_HINT':
      return { ...state, showHint: action.payload };

    case 'SET_CATEGORY':
      return { ...state, category: action.payload };

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

  const startSession = useCallback(() => {
    dispatch({ type: 'START_SESSION' });
  }, []);

  const endSession = useCallback(() => {
    dispatch({ type: 'END_SESSION' });
  }, []);

  const setCurrentQuestion = useCallback((question: Question | null) => {
    dispatch({ type: 'SET_CURRENT_QUESTION', payload: question });
  }, []);

  const setLoadingQuestion = useCallback((isLoading: boolean) => {
    dispatch({ type: 'SET_LOADING_QUESTION', payload: isLoading });
  }, []);

  const setShowHint = useCallback((show: boolean) => {
    dispatch({ type: 'SET_SHOW_HINT', payload: show });
  }, []);

  const setCategory = useCallback((category: Category | null) => {
    dispatch({ type: 'SET_CATEGORY', payload: category });
  }, []);

  const startRecording = useCallback(() => {
    dispatch({ type: 'START_RECORDING' });
  }, []);

  const stopRecording = useCallback(() => {
    dispatch({ type: 'STOP_RECORDING' });
  }, []);

  const setError = useCallback((errorMessage: string | null) => {
    dispatch({ type: 'SET_ERROR', payload: errorMessage });
  }, []);

  const fetchNextQuestion = useCallback(async () => {
    try {
      setLoadingQuestion(true);
      setError(null);
      setShowHint(false);

      const question = await fetchRandomQuestion(state.category?.id);

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
    state.category,
    setCurrentQuestion,
    setError,
    setLoadingQuestion,
    setShowHint,
  ]);

  // Fetch initial question when session started and category was set
  useEffect(() => {
    if (state.sessionStarted && state.category !== undefined) {
      fetchNextQuestion();
    }
  }, [state.sessionStarted, state.category, fetchNextQuestion]);

  const contextValue: PracticeSessionContextProps = useMemo(
    () => ({
      state,
      startSession,
      endSession,
      setCurrentQuestion,
      setLoadingQuestion,
      setShowHint,
      setCategory,
      startRecording,
      stopRecording,
      setError,
      fetchNextQuestion,
    }),
    [
      state,
      startSession,
      endSession,
      setCurrentQuestion,
      setLoadingQuestion,
      setShowHint,
      setCategory,
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
