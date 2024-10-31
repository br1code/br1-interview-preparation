# Client

This project is a **Next.js** application that serves as the frontend for the interview preparation platform. It allows users to practice answering questions using their camera and microphone, submit new questions, and review recorded answers.

- [Client](#client)
  - [Pages Overview](#pages-overview)
    - [Homepage (`/`) ✅](#homepage--)
    - [Practice Session Page (`/practice`) ✅](#practice-session-page-practice-)
    - [Questions List Page (`/questions`) ✅](#questions-list-page-questions-)
    - [Add Question Page (`/questions/add`) ✅](#add-question-page-questionsadd-)
    - [Question Detail Page (`/questions/{id}`) ✅](#question-detail-page-questionsid-)
    - [Answer Detail Page (`/answers/{id}`) ✅](#answer-detail-page-answersid-)
    - [Add Category Page (`/categories/add`) ✅](#add-category-page-categoriesadd-)
    - [Categories List Page (`/categories`) ✅](#categories-list-page-categories-)
    - [Category Detail Page (`/categories/{id}`) ✅](#category-detail-page-categoriesid-)
  - [Getting Started](#getting-started)
  - [Contributing](#contributing)
  - [License](#license)

## Pages Overview

### Homepage (`/`) ✅

The homepage serves as the starting point for users to begin practicing interview questions.

- **Category Selection Dropdown** ✅
  - Fetches categories from the backend and displays them in a dropdown menu.
    - **Endpoint:** `GET /api/categories`
  - Users can select a specific category or choose "All Categories" to include questions from all categories.
- **Start Button** ✅
  - Begins the practice session with questions from the selected category.
  - Redirects the user to the `/practice` route.
- **Edit Categories Button**
  - Redirects the user to the `/categories` route. ✅
  - **TODO**: place this button in a navbar.
- **Edit Questions Button**
  - Redirects the user to the `/questions` route. ✅
  - **TODO**: place this button in a navbar.

### Practice Session Page (`/practice`) ✅

This page allows users to practice answering questions and record their responses.

- **Introduction Screen** ✅
  - Explains how the Practice Session works. Displays the selected category (if any).
- **Start Session Button** ✅
  - Starts the Practice Session using the selected category (if any).
- **Question Display** ✅
  - Shows a random question from the selected category.
    - **Endpoint:** `GET /api/questions/random?categoryId={Guid}`
- **Countdown Timer** ✅
  - A 5-second countdown before the recording starts, giving users time to prepare.
- **Hint Button** ✅
  - Allows users to show or hide a hint associated with the question.
- **Recording Interface** ✅
  - Automatically starts recording the user's camera and microphone after the countdown.
  - Displays a "Submit Answer" button to end the recording.
  - Upon stopping, the video is uploaded to the backend as a new answer.
    - **Endpoint:** `POST /api/answers`
- **Skip Question Button** ✅
  - Allows users to skip the current question and receive another random question.
- **Loop Mechanism** ✅
  - After each recording or skipped question, a new random question appears.
  - The session continues indefinitely until the user decides to stop.
- **End Screen** ✅
  - Displays session statistics such as:
    - Number of questions answered (with links to each Question)
    - Number of questions skipped (with links to each Question)
    - Total time spent (minutes, seconds)
  - Provides buttons to start a new session or return to the homepage. ✅

### Questions List Page (`/questions`) ✅

This page lists all available questions and allows for management actions.

- **Add Question Button** ✅
  - Redirects the user to the `/questions/add` route.
- **Category Filter** ✅
  - Users can filter the questions by category using a dropdown.
    - **Endpoint:** `GET /api/categories`
- **Question List** ✅
  - Displays all questions, optionally filtered by the selected category.
    - **Endpoint:** `GET /api/questions?categoryId={Guid}`
  - Each question includes:
    - Content, Category ✅
    - Number of submitted answers for the question. ✅
    - A link to its detail page. ✅
    - A "Delete" button to remove the question and its associated answers. ✅
      - Displays a confirmation modal before deletion.
      - **Endpoint:** `DELETE /api/questions/{id}`

### Add Question Page (`/questions/add`) ✅

Allows users to add new questions to the platform.

- **Question Form** ✅
  - **Question Content:** A text area input for the question text. ✅
  - **Question Hint:** An optional text area input for a hint. ✅
  - **Category Selection** ✅
    - Fetches categories from the backend to select the appropriate category.
    - **Endpoint:** `GET /api/categories`
  - **Submit Button** ✅
    - Adds the new question to the database.
    - **Endpoint:** `POST /api/questions`
  - Provides buttons to edit the last question added or return to the homepage. ✅

### Question Detail Page (`/questions/{id}`) ✅

Provides detailed information about a specific question and its submitted answers.

- **Fetch Question and Answers** ✅
  - Retrieves the question details and associated answers.
    - **Endpoint:** `GET /api/questions/{id}`
- **Question Content and Hint** ✅
  - Displays the question content and hint in editable text areas.
  - Users can update these fields as needed.
- **Category Selection Dropdown** ✅
  - Allows users to change the category of the question.
    - **Endpoint:** `GET /api/categories`
- **Update Question Button** ✅
  - Saves any changes made to the question content, hint, or category.
    - **Endpoint:** `PUT /api/questions/{id}`
- **Delete Question Button** ✅
  - Displays a confirmation modal before deleting the question. The users gets redirected to the Questions List page.
    - **Endpoint:** `DELETE /api/questions/{id}`
- **Submitted Answers** ✅
  - Lists all video recordings submitted for this question, sorted by creation date.
  - Each answer includes:
    - A link to display the answer metadata and play back the video. The text of the link displays the creation date.
      - **Endpoint:** `GET /api/answers/{id}`
- Provides a button to return to the homepage. ✅

### Answer Detail Page (`/answers/{id}`) ✅

Displays a video player with the recorded answer. Allows users to delete the answer.

- Displays the submission date (createdAt). ✅
- A "Delete" button to remove the answer and its associated video file. ✅
  - Displays a confirmation modal before deletion.
  - **Endpoint:** `DELETE /api/answers/{id}`
- A link to the Question Details page. ✅

### Add Category Page (`/categories/add`) ✅

Allows users to add new categories to the platform.

- **Category Form** ✅
  - **Category Name:** A text area input for the category name. ✅
  - **Submit Button** ✅
    - Adds the new category to the database.
    - **Endpoint:** `POST /api/categories`
  - Provides buttons to edit the last category added or return to the homepage. ✅

### Categories List Page (`/categories`) ✅

This page lists all available categories and allows for management actions.

- **Add Category Button** ✅
  - Redirects the user to the `/categories/add` route.
- **Categories List** ✅
  - Displays all categories in a table.
    - **Endpoint:** `GET /api/categories/detailed`
  - Each item in the table includes:
    - The name of the category. ✅
    - The number of submitted questions for the category. ✅
    - A link to its detail page. ✅
    - A "Delete" button to remove the category and its associated question/answers. ✅
      - Displays a confirmation modal before deletion.
      - **Endpoint:** `DELETE /api/categories/{id}`

### Category Detail Page (`/categories/{id}`) ✅

Display information about a specific category.

- **Fetch Category** ✅
  - Retrieves the category details.
    - **Endpoint:** `GET /api/categories/{id}`
- **Category Name** ✅
  - Displays the category name in a editable text area. Users can update this fields as needed.
- **Update Category Button** ✅
  - Saves any changes made to the category name.
    - **Endpoint:** `PUT /api/categories/{id}`
- **Delete Category Button** ✅
  - Displays a confirmation modal before deleting the category. The users gets redirected to the Categories List page.
    - **Endpoint:** `DELETE /api/categories/{id}`
- Provides a button to return to the homepage. ✅

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
   API_URL=https://localhost:7011/api
   NEXT_PUBLIC_API_URL=https://localhost:7011/api
   ```

   Replace `https://localhost:7011/api` with the URL where your backend API is running.

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
