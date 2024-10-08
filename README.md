# Br1 Interview Preparation

This is a full-stack application consisting of a .NET 8 backend API, a Next.js frontend, and a PostgreSQL database. The application allows users to prepare for interviews by answering questions using the microphone + camera. The recordings with the answers will be stored as video files so they can be reviewed later. The questions must be submitted by the users.

## Prerequisites

- [Docker](https://www.docker.com/get-started) installed on your machine.

## Setting Up Environment Variables

1. Ensure that a `.env` file exists in the root directory.

```bash
touch .env
```

2. Update the content of the `.env` file:

```env
TODO
```

3. Important: Ensure that the `.env` file is saved in the same directory as the `docker-compose.yml` file.

## Building and Running the Application

1. Build and start the application using Docker Compose:

```bash
docker-compose up --build
```

- This command will build the Docker images and start all the services defined in the `docker-compose.yml` file.
- The first build may take several minutes.

## Accessing the Application

Once the application is running, you can access it via the following URLs:

- Frontend Application:

```
http://localhost:3000
```

Backend API Swagger UI:

```
http://localhost:5000/swagger
```

## Stopping the Application

To stop the application and remove the containers, run:

```bash
docker-compose down
```
