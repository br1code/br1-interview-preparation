# Client

This project is a **Next.js** application that serves as the frontend for the interview preparation platform. It allows users to practice answering questions using their camera and microphone, submit new questions, and review recorded answers.

- [Client](#client)
  - [Pages Overview](#pages-overview)
    - [Homepage (`/`)](#homepage-)
    - [Practice Session Page (`/practice`)](#practice-session-page-practice)
    - [Questions List Page (`/questions`)](#questions-list-page-questions)
    - [Add Question Page (`/questions/add`)](#add-question-page-questionsadd)
    - [Question Detail Page (`/questions/{id}`)](#question-detail-page-questionsid)
  - [API Endpoints](#api-endpoints)
  - [Getting Started](#getting-started)
  - [Contributing](#contributing)
  - [License](#license)

## Pages Overview

### Homepage (`/`)

The homepage serves as the starting point for users to begin practicing interview questions.

- **Category Selection Dropdown**
  - Fetches categories from the backend and displays them in a dropdown menu.
    - **Endpoint:** `GET /api/categories`
  - Users can select a specific category or choose "All Categories" to include questions from all categories.
- **Start Button**
  - Begins the practice session with questions from the selected category.
  - Redirects the user to the `/practice` route.

### Practice Session Page (`/practice`)

This page allows users to practice answering questions and record their responses.

- **Question Display**
  - Shows a random question from the selected category.
    - **Endpoint:** `GET /api/questions/random?categoryId={Guid}`
- **Countdown Timer**
  - A 5-second countdown before the recording starts, giving users time to prepare.
- **Hint Button**
  - Allows users to show or hide a hint associated with the question.
- **Recording Interface**
  - Automatically starts recording the user's camera and microphone after the countdown.
  - Displays a "Submit Answer" button to end the recording.
  - Upon stopping, the video is uploaded to the backend as a new answer.
    - **Endpoint:** `POST /api/answers`
- **Skip Button**
  - Allows users to skip the current question and receive another random question.
- **Loop Mechanism**
  - After each recording or skipped question, a new random question appears.
  - The session continues indefinitely until the user decides to stop.
- **End Screen**
  - Displays session statistics such as:
    - Number of questions answered
    - Number of questions skipped
    - Total time spent
  - Provides an option to review recorded answers or return to the homepage.

### Questions List Page (`/questions`)

This page lists all available questions and allows for management actions.

- **Category Filter**
  - Users can filter the questions by category using a dropdown.
    - **Endpoint:** `GET /api/categories`
- **Question List**
  - Displays all questions, optionally filtered by the selected category.
    - **Endpoint:** `GET /api/questions?categoryId={Guid}`
  - Shows the number of submitted answers for each question.
  - Each question includes:
    - A link to its detail page.
    - A "Delete" button to remove the question and its associated answers.
      - Displays a confirmation modal before deletion.
      - **Endpoint:** `DELETE /api/questions/{id}`

### Add Question Page (`/questions/add`)

Allows users to add new questions to the platform.

- **Question Form**
  - **Question Content:** A text area input for the question text.
  - **Question Hint:** An optional text area input for a hint.
  - **Category Selection:**
    - Fetches categories from the backend to select the appropriate category.
    - **Endpoint:** `GET /api/categories`
  - **Submit Button**
    - Adds the new question to the database.
    - **Endpoint:** `POST /api/questions`

### Question Detail Page (`/questions/{id}`)

Provides detailed information about a specific question and its submitted answers.

- **Fetch Question and Answers**
  - Retrieves the question details and associated answers.
    - **Endpoint:** `GET /api/questions/{id}`
- **Question Content and Hint**
  - Displays the question content and hint in editable text areas.
  - Users can update these fields as needed.
- **Category Selection Dropdown**
  - Allows users to change the category of the question.
    - **Endpoint:** `GET /api/categories`
- **Update Question Button**
  - Saves any changes made to the question content, hint, or category.
    - **Endpoint:** `PUT /api/questions/{id}`
- **Submitted Answers**
  - Lists all video recordings submitted for this question, sorted by creation date.
  - Each answer includes:
    - A link to play back the video.
      - **Endpoint:** `GET /api/answers/{id}`
    - A "Delete" button to remove the answer and its associated video file.
      - Displays a confirmation modal before deletion.
      - **Endpoint:** `DELETE /api/answers/{id}`

## API Endpoints

For reference, here are the key API endpoints used by the client application:

- **Categories**
  - `GET /api/categories`: Retrieves all categories.
- **Questions**
  - `GET /api/questions?categoryId={Guid}`: Retrieves questions, optionally filtered by category.
  - `GET /api/questions/random?categoryId={Guid}`: Retrieves a random question, optionally filtered by category.
  - `GET /api/questions/{id}`: Gets a specific question and its answers.
  - `POST /api/questions`: Adds a new question.
  - `PUT /api/questions/{id}`: Updates a question.
  - `DELETE /api/questions/{id}`: Deletes a question and its answers.
- **Answers**
  - `POST /api/answers`: Uploads a new answer video for a question.
  - `GET /api/answers/{id}`: Streams the video associated with the specific answer ID.
  - `DELETE /api/answers/{id}`: Deletes an answer and its associated video file.

## Getting Started

To set up and run the client application locally, follow these steps:

1. **Clone the Repository**

   ```bash
   git clone https://github.com/br1code/br1-interview-preparation.git
   ```

2. **Navigate to the Project Directory**

   ```bash
   cd client/br1-interview-preparation
   ```

3. **Install Dependencies**

   ```bash
   npm install
   ```

4. **Set Up Environment Variables**

   Create a `.env.local` file in the root directory and add any necessary environment variables. For example:

   ```env
   NEXT_PUBLIC_API_BASE_URL=http://localhost:5000
   ```

   Replace `http://localhost:5000` with the URL where your backend API is running.

5. **Run the Development Server**

   ```bash
   npm run dev
   ```

   The application will start on [http://localhost:3000](http://localhost:3000).

6. **Build for Production**

   To build the application for production:

   ```bash
   npm run build
   ```

   Then, start the production server:

   ```bash
   npm start
   ```

## Contributing

Contributions are welcome! If you'd like to contribute to this project, please follow these steps:

1. **Fork the Repository**

   Click the "Fork" button at the top right of the repository page to create a copy of the repository on your account.

2. **Create a Feature Branch**

   ```bash
   git checkout -b feature/YourFeatureName
   ```

3. **Commit Your Changes**

   ```bash
   git commit -am 'Add some feature'
   ```

4. **Push to the Branch**

   ```bash
   git push origin feature/YourFeatureName
   ```

5. **Open a Pull Request**

   Go to your forked repository and click the "New Pull Request" button.

## License

This project is licensed under the **MIT License**. See the [LICENSE](LICENSE) file for details.

---

**Note:** Ensure that the backend API is running and accessible at the URL specified in your environment variables. For more information about setting up and running the backend server, refer to the [Server README](../server/README.md).

If you encounter any issues or have questions, please open an issue on the repository or contact the project maintainers.
