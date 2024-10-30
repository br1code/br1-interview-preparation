# Server

This project is a .NET 8 backend API that connects to a PostgreSQL database. It provides endpoints for managing categories, questions, and answers, including uploading and streaming video files associated with answers.

## Table of Contents

- [Server](#server)
  - [Table of Contents](#table-of-contents)
  - [Overview](#overview)
  - [API Endpoints](#api-endpoints)
    - [Categories](#categories)
      - [`GET /api/categories` ✅](#get-apicategories-)
      - [`GET /api/categories/detailed` ✅](#get-apicategoriesdetailed-)
      - [`GET /api/categories/{id}` ✅](#get-apicategoriesid-)
      - [`POST /api/categories` ✅](#post-apicategories-)
      - [`PUT /api/categories/{id}` ✅](#put-apicategoriesid-)
      - [`DELETE /api/categories/{id}` ✅](#delete-apicategoriesid-)
    - [Questions](#questions)
      - [`GET /api/questions?categoryId={Guid}` ✅](#get-apiquestionscategoryidguid-)
      - [`GET /api/questions/random?categoryId={Guid}` ✅](#get-apiquestionsrandomcategoryidguid-)
      - [`GET /api/questions/{id}` ✅](#get-apiquestionsid-)
      - [`POST /api/questions` ✅](#post-apiquestions-)
      - [`PUT /api/questions/{id}` ✅](#put-apiquestionsid-)
      - [`DELETE /api/questions/{id}` ✅](#delete-apiquestionsid-)
    - [Answers](#answers)
      - [`POST /api/answers` ✅](#post-apianswers-)
      - [`GET /api/answers/{id}` ✅](#get-apianswersid-)
      - [`GET /api/answers/{id}/metadata` ✅](#get-apianswersidmetadata-)
      - [`DELETE /api/answers/{id}` ✅](#delete-apianswersid-)
  - [Database Entities](#database-entities)
    - [Categories](#categories-1)
    - [Questions](#questions-1)
    - [Answers](#answers-1)
  - [Getting Started](#getting-started)
    - [Prerequisites](#prerequisites)
    - [Configuration](#configuration)
    - [Running the Application](#running-the-application)
    - [Migration Scripts](#migration-scripts)
  - [Contributing](#contributing)
  - [License](#license)

## Overview

The server application provides a RESTful API for:

- **Categories**: Managing categories for organizing questions.
- **Questions**: CRUD operations for interview questions.
- **Answers**: Handling video uploads and streaming for recorded answers.

---

## API Endpoints

### Categories

#### `GET /api/categories` ✅

Retrieves all categories.

- **Response:**

  - **Status:** `200 OK`
  - **Body:**

    ```json
    [
      {
        "id": "uuid",
        "name": "Databases"
      },
      {
        "id": "uuid",
        "name": "Design Patterns"
      }
    ]
    ```

#### `GET /api/categories/detailed` ✅

Retrieves all categories and the number of its submitted questions.

- **Response:**

  - **Status:** `200 OK`
  - **Body:**

    ```json
    [
      {
        "id": "uuid",
        "name": "Databases",
        "questionsCount": 1
      },
      {
        "id": "uuid",
        "name": "Design Patterns",
        "questionsCount": 1
      }
    ]
    ```

#### `GET /api/categories/{id}` ✅

Retrieves a specific category.

- **Response:**

  - **Status:** `200 OK`
  - **Body:**

    ```json
    {
      "id": "uuid",
      "name": "Databases"
    }
    ```

#### `POST /api/categories` ✅

Adds a new category.

- **Request:**

  - **Headers:**

    - `Content-Type: application/json`

  - **Body:**

    ```json
    {
      "name": "Category Name"
    }
    ```

- **Response:**

  - **Status:** `201 Created`
  - **Body:**

    ```json
    "uuid"
    ```

#### `PUT /api/categories/{id}` ✅

Updates an existing category.

- **Parameters:**

  - `id` (required): UUID of the category to update.

- **Request:**

  - **Headers:**

    - `Content-Type: application/json`

  - **Body:**

    ```json
    {
      "name": "Category Name"
    }
    ```

- **Response:**

  - **Status:** `200 OK`
  - **Body:**

    ```json
    {
      "id": "uuid",
      "name": "Category Name"
    }
    ```

#### `DELETE /api/categories/{id}` ✅

Deletes a category and its associated questions/answers.

- **Parameters:**

  - `id` (required): UUID of the category to delete.

- **Response:**

  - **Status:** `204 No Content`

---

### Questions

#### `GET /api/questions?categoryId={Guid}` ✅

Retrieves questions and its answers, optionally filtered by category.

- **Parameters:**

  - `categoryId` (optional): UUID of the category to filter questions.

- **Response:**

  - **Status:** `200 OK`
  - **Body:**

    ```json
    [
      {
        "id": "uuid",
        "categoryId": "uuid",
        "content": "Question content",
        "hint": "Optional hint",
        "answersCount": 5
      }
    ]
    ```

#### `GET /api/questions/random?categoryId={Guid}` ✅

Retrieves a random question, optionally filtered by category.

- **Parameters:**

  - `categoryId` (optional): UUID of the category to filter questions.

- **Response:**

  - **Status:** `200 OK`
  - **Body:**

    ```json
    {
      "id": "uuid",
      "categoryId": "uuid",
      "content": "Random question content",
      "hint": "Optional hint"
    }
    ```

#### `GET /api/questions/{id}` ✅

Gets a specific question and its answers.

- **Parameters:**

  - `id` (required): UUID of the question.

- **Response:**

  - **Status:** `200 OK`
  - **Body:**

    ```json
    {
      "question": {
        "id": "uuid",
        "categoryId": "uuid",
        "content": "Question content",
        "hint": "Optional hint"
      },
      "answers": [
        {
          "id": "uuid",
          "questionId": "uuid",
          "videoFileName": "123.webm",
          "createdAt": "Date"
        }
      ]
    }
    ```

#### `POST /api/questions` ✅

Adds a new question.

- **Request:**

  - **Headers:**

    - `Content-Type: application/json`

  - **Body:**

    ```json
    {
      "categoryId": "uuid",
      "content": "New question content",
      "hint": "Optional hint"
    }
    ```

- **Response:**

  - **Status:** `201 Created`
  - **Body:**

    ```json
    "uuid"
    ```

#### `PUT /api/questions/{id}` ✅

Updates an existing question.

- **Parameters:**

  - `id` (required): UUID of the question to update.

- **Request:**

  - **Headers:**

    - `Content-Type: application/json`

  - **Body:**

    ```json
    {
      "categoryId": "uuid",
      "content": "Updated question content",
      "hint": "Updated hint"
    }
    ```

- **Response:**

  - **Status:** `200 OK`
  - **Body:**

    ```json
    {
      "id": "uuid",
      "categoryId": "uuid",
      "content": "Updated question content",
      "hint": "Updated hint"
    }
    ```

#### `DELETE /api/questions/{id}` ✅

Deletes a question and its associated answers.

- **Parameters:**

  - `id` (required): UUID of the question to delete.

- **Response:**

  - **Status:** `204 No Content`

---

### Answers

#### `POST /api/answers` ✅

Uploads a new answer video for a question.

- **Request:**

  - **Headers:**

    - `Content-Type: multipart/form-data`

  - **Body:**

    - `questionId` (form field): UUID of the question being answered.
    - `videoFile` (form field): The video file to upload.

- **Response:**

  - **Status:** `201 Created`
  - **Body:**

    ```json
    "uuid"
    ```

#### `GET /api/answers/{id}` ✅

Streams the video associated with the specific answer ID.

- **Parameters:**

  - `id` (required): UUID of the answer.

- **Response:**

  - **Status:** `200 OK`
  - **Headers:**

    - `Content-Type: video/webm` (or appropriate video MIME type)
    - `Accept-Ranges: bytes`

  - **Body:**
    - Binary video data.

- **Note:** This endpoint supports video streaming with byte-range requests.

#### `GET /api/answers/{id}/metadata` ✅

Gets an answer and its metadata.

- **Parameters:**

  - `id` (required): UUID of the answer.

- **Response:**

  - **Status:** `200 OK`
  - **Body:**

    ```json
    {
      "id": "uuid",
      "questionId": "uuid",
      "videoFileName": "123.webm"
    }
    ```

#### `DELETE /api/answers/{id}` ✅

Deletes an answer and its associated video file.

- **Parameters:**

  - `id` (required): UUID of the answer to delete.

- **Response:**

  - **Status:** `204 No Content`

---

## Database Entities

### Categories

- **Fields:**
  - `id`: UUID (Primary Key)
  - `name`: String

### Questions

- **Fields:**
  - `id`: UUID (Primary Key)
  - `categoryId`: UUID (Foreign key to Categories)
  - `content`: Text
  - `hint`: Text (Nullable)
  - `createdAt`: Timestamp
  - `updatedAt`: Timestamp

### Answers

- **Fields:**
  - `id`: UUID (Primary Key)
  - `questionId`: UUID (Foreign key to Questions)
  - `videoFilename`: String
  - `createdAt`: Timestamp
  - `updatedAt`: Timestamp

---

## Getting Started

### Prerequisites

- **.NET 8 SDK**: Ensure you have the .NET 8 SDK installed.
- **PostgreSQL**: A running PostgreSQL instance.
- **Entity Framework Core CLI**: For running migrations.

### Configuration

1. **Connection String**

   Update the `appsettings.json` file with your PostgreSQL connection string and the file path for storing video files:

   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Database=YourDatabase;Username=YourUsername;Password=YourPassword"
     },
     "VideoStorage": {
       "FilePath": "/videos/"
     }
   }
   ```

2. **Environment Variables**

   Alternatively, you can use environment variables to set the connection string.

### Running the Application

1. **Apply Migrations**

   Use Entity Framework Core to apply migrations and create the database schema.

   ```bash
   dotnet ef database update
   ```

2. **Run the Application**

   ```bash
   dotnet run
   ```

   The API will be accessible at `http://localhost:5000` (or the configured port).

### Migration Scripts

Examples of how to manage migrations:

- **Add a Migration**

  ```bash
  dotnet ef migrations add InitialCreate --project Br1InterviewPreparation.Infrastructure --startup-project Br1InterviewPreparation.API
  ```

- **Update the Database**

  ```bash
  dotnet ef database update --project Br1InterviewPreparation.Infrastructure --startup-project Br1InterviewPreparation.API
  ```

---

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

---

## License

This project is licensed under the **MIT License**. See the [LICENSE](LICENSE) file for details.
