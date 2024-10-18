import { useState, useRef, useCallback } from 'react';

interface UseMediaRecorderResult {
  recordedChunksRef: React.MutableRefObject<Blob[]>;
  permissionError: string | null;
  startRecording: (videoElement: HTMLVideoElement) => Promise<void>;
  stopRecording: () => Promise<void>;
  resetRecordedChunks: () => void;
}

const useMediaRecorder = (): UseMediaRecorderResult => {
  const mediaStreamRef = useRef<MediaStream | null>(null);
  const mediaRecorderRef = useRef<MediaRecorder | null>(null);
  const recordedChunksRef = useRef<Blob[]>([]);
  const [permissionError, setPermissionError] = useState<string | null>(null);

  const startRecording = useCallback(async (videoElement: HTMLVideoElement) => {
    try {
      const stream = await navigator.mediaDevices.getUserMedia({
        video: true,
        audio: true,
      });
      mediaStreamRef.current = stream;

      // Display the live video preview
      videoElement.srcObject = stream;
      await videoElement.play();

      const options = { mimeType: 'video/webm; codecs=vp8,opus' };
      if (!MediaRecorder.isTypeSupported(options.mimeType)) {
        console.error(`${options.mimeType} is not supported`);
        setPermissionError(`${options.mimeType} is not supported`);
        return;
      }

      const mediaRecorder = new MediaRecorder(stream, options);
      mediaRecorderRef.current = mediaRecorder;

      // Reset recorded chunks
      recordedChunksRef.current = [];

      mediaRecorder.ondataavailable = (event) => {
        if (event.data.size > 0) {
          recordedChunksRef.current.push(event.data);
        }
      };

      mediaRecorder.start();
    } catch (error) {
      console.error('Error accessing media devices:', error);
      setPermissionError(
        'Could not access camera and microphone. Please check your permissions.'
      );
    }
  }, []);

  const stopRecording = useCallback((): Promise<void> => {
    return new Promise((resolve) => {
      if (mediaRecorderRef.current) {
        const recorder = mediaRecorderRef.current;

        const handleStop = () => {
          // Stop all media tracks
          mediaStreamRef.current?.getTracks().forEach((track) => track.stop());
          resolve();
        };

        if (recorder.state === 'inactive') {
          handleStop();
        } else {
          recorder.onstop = handleStop;
          recorder.stop();
        }
      } else {
        resolve();
      }
    });
  }, []);

  const resetRecordedChunks = useCallback(() => {
    recordedChunksRef.current = [];
  }, []);

  return {
    recordedChunksRef,
    permissionError,
    startRecording,
    stopRecording,
    resetRecordedChunks,
  };
};

export default useMediaRecorder;
