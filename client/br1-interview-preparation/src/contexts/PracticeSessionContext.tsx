import { createContext, FC, ReactNode, useContext, useReducer } from 'react';
import { Question } from '@/types';

interface PracticeSessionState {
  currentQuestion: Question | null;
  isRecording: boolean;
  categoryId: string | null;
}

type PracticeSessionAction =
  | { type: 'SET_CURRENT_QUESTION'; payload: Question | null }
  | { type: 'SET_CATEGORY_ID'; payload: string }
  | { type: 'START_RECORDING' }
  | { type: 'STOP_RECORDING' };

interface PracticeSessionContextProps {
  state: PracticeSessionState;
  setCurrentQuestion: (question: Question | null) => void;
  setCategoryId: (categoryId: string) => void;
  startRecording: () => void;
  stopRecording: () => void;
}

const initialState: PracticeSessionState = {
  currentQuestion: null,
  isRecording: false,
  categoryId: null,
};

const reducer = (
  state: PracticeSessionState,
  action: PracticeSessionAction
): PracticeSessionState => {
  switch (action.type) {
    case 'SET_CURRENT_QUESTION':
      return { ...state, currentQuestion: action.payload };
    case 'SET_CATEGORY_ID':
      return { ...state, categoryId: action.payload };
    case 'START_RECORDING':
      return { ...state, isRecording: true };
    case 'STOP_RECORDING':
      return { ...state, isRecording: false };
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

  const setCurrentQuestion = (question: Question | null) => {
    dispatch({ type: 'SET_CURRENT_QUESTION', payload: question });
  };

  const setCategoryId = (categoryId: string) => {
    dispatch({ type: 'SET_CATEGORY_ID', payload: categoryId });
  };

  const startRecording = () => {
    dispatch({ type: 'START_RECORDING' });
  };

  const stopRecording = () => {
    dispatch({ type: 'STOP_RECORDING' });
  };

  const contextValue: PracticeSessionContextProps = {
    state,
    setCurrentQuestion,
    setCategoryId,
    startRecording,
    stopRecording,
  };

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
