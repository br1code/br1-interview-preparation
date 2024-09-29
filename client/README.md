# Client

This project is a Next js application.

## Pages

- Homepage (`/`):

  - Category Selection dropdown:
    - Fetch categories from the backend and display them in a dropdown.
    - Option to select "All Categories" if none is chosen.
  - Start button:
    - Begins the practice session with questions from the selected category or any category if none is selected (redirects the user to the `/practice` route).

- Practice Session Page (`/practice`):

  - Question Display:
    - Shows a random question from the selected category.
  - Countdown Timer:
    - Waits for 5 seconds before starting the recording.
  - Hint Button:
    - Allows the user to show/hide a hint. This shouldn't be possible during recording.
  - Recording interface:
    - Automatically starts recording after the countdown.
    - Displays a "Stop" button for the user to end the recording.
  - Skip Button:
    - Allows the user to skip a question.
  - Loop Mechanism:
    - After stopping and uploading the video, another random question appears.
    - Continues indefinitely until the user decides to stop.
    - To avoid repeating same questions during a game session, the app should store the id of the answered questions in some local state. These IDs will be used to fetch a question again if it was already answered.
  - End Screen:
    - Shows the session stats (answered questions, skipped questions, time, etc).

- Questions List Page (`/questions`):

  - Category Filter:
    - Allows users to filter questions by category.
  - Question List:
    - Displays all questions with the number of submitted answers.
    - Each question links to its detail page.
    - Each question has a Delete button that removes it from the database, including all submitted answers. Displays a confirmation modal first.

- Add Question Page (`/questions/add`):

  - Question form:
    - Text Area input for the question content.
    - Text Area input for the question hint.
    - Submit button to add the question to the database.

- Question Detail Page (`/questions/{id}`):
  - Question Content and Hint:
    - Shows the question Content and Hint as enabled Text Area inputs.
  - Update Question button:
    - Allows the user to edit the Content and Hint inputs, and update the question.
  - Submitted Answers:
    - Lists all video recordings submitted for this question, sorted by date of creation.
    - Each answer links to a video playback.
    - Each answer has a Delete button that removes it from the database, including its associated video file. Displays a confirmation modal first.
