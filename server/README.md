# Server

This project is a .NET 8 backend API that connects to a PostgreSQL database.

## API Endpoints

Categories:

- `GET /api/categories`: Retrieves all categories. ✅

Questions:

- `GET /api/questions?categoryId={Guid}`: Retrieve questions, optionally filtered by category. ✅
- `GET /api/questions/random?categoryId={Guid}`: Retrieve a random question, optionally filtered by category. ✅
- `GET /api/questions/{id}`: Get a specific question and its answers. ✅
- `POST /api/questions`: Add a new question. ✅
- `PUT /api/questions/{id}`: Update a question. ✅
- `DELETE /api/questions/{id}`: Delete a question and its answers. ✅

Answers:

- `POST /api/answers`: Upload a new answer video for a question. 🔨 (TODO: we are not storing the video files yet)
- `GET /api/answers/{id}`: Stream the video associated with the specific answer ID. ✅
- `GET /api/answers/{id}/metadata`: Stream the video associated with the specific answer ID. ✅
- `DELETE /api/answers/{id}`: Delete an answer and its associated video file. (TODO: soft delete?)

## Database Entities

Categories:

- `id`: UUID
- `name`: String

Questions:

- `id`: UUID
- `category_id`: UUID (Foreign key to Categories)
- `content`: Text
- `hint`: Text (TBD), Nullable/Optional
- `created_at`: Timestamp
- `updated_at`: Timestamp

Answers:

- `id`: UUID
- `question_id`: UUID (Foreign key to Questions)
- `video_filename`: String
- `created_at`: Timestamp
- `updated_at`: Timestamp

## Migrations scripts

Examples:

```bash
dotnet ef migrations add initialCreate --project Br1InterviewPreparation.Infrastructure --startup-project Br1InterviewPreparation.API

dotnet ef database update --project Br1InterviewPreparation.Infrastructure --startup-project Br1InterviewPreparation.API
```
