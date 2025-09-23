# KvizAPI Solution

## Overview
KvizAPI is a modular quiz application built with ASP.NET Core (.NET 8). It provides a RESTful API for managing quizzes, questions, and users, and supports extensibility via plugins. The solution is organized into multiple projects for clear separation of concerns.

## Projects

### 1. KvizAPI
- **Type:** ASP.NET Core Web API
- **Purpose:** Main application backend, exposes endpoints for quiz and question management, user authentication, and more.
- **Key Features:**
  - Quiz and question CRUD operations
  - User management and authentication
  - Caching and dependency injection
  - Middleware support

### 2. Plugins
- **Type:** .NET Class Library
- **Purpose:** Provides extension points and DTOs for plugin development, enabling additional features or integrations.

### 3. KvizApi.Tests
- **Type:** xUnit Test Project
- **Purpose:** Contains unit and integration tests for the API and services, ensuring code quality and correctness.
- **Key Features:**
  - In-memory and containerized database testing
  - Service and controller tests

## Technologies Used
- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- xUnit (testing)

## Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [PostgreSQL](https://www.postgresql.org/)


### Running the API
1. Navigate to the `KvizAPI` project directory:
   ```sh
   cd KvizAPI
   ```
2. Run the API:
   ```sh
   dotnet run
   ```

### Running Tests
1. Navigate to the solution root or `KvizApi.Tests` directory.
2. Run tests:
   ```sh
   dotnet test
   ```

## Extending with Plugins
- Add new features by creating projects that reference the `Plugins` library.
- Implement custom DTOs or services as needed.



## Features
- User registration and login with JWT authentication
- CRUD operations for quizzes and questions
- Global query filters for soft-delete
- Entity Framework Core with PostgreSQL
- Swagger UI with JWT Authorize support
- Error handling middleware
- Seed data for questions
- CI/CD pipeline with build/test steps


### Configuration
1. **Connection String & Auth Options**
   - Edit `appsettings.json` and/or `appsettings.Development.json`:
     ```json
     {
       "ConnectionStrings": {
         "DefaultConnection": "Host=localhost;Port=5432;Username=youruser;Password=yourpass;Database=QuizDB"
       },
       "AuthOptions": {
         "SecretKey": "your-very-strong-key",
         "Issuer": "QuizAPI",
         "Audience": "QuizClients"
       }
     }
     ```
2. **Set Environment**
   - For development, set `ASPNETCORE_ENVIRONMENT=Development`.
3. **Create database and run migrations**

### Build & Run
```bash
# Restore, build, and run migrations/seed automatically
cd KvizAPI
 dotnet run
```

### API Documentation
- Swagger UI: [https://localhost:59217/swagger/index.html](https://localhost:59217/swagger/index.html)
- Use the "Authorize" button to enter a JWT for protected endpoints.

## API Endpoints

### Auth
- `POST /api/auth/register` Register a new user
- `POST /api/auth/login` Login and get JWT

### Quizzes
- `GET /api/quiz` List all quizzes (auth required)
- `POST /api/quiz` Create quiz (auth required)
- `PUT /api/quiz/{id}` Update quiz (auth required)
- `DELETE /api/quiz/{id}` Delete quiz (auth required)

### Questions
- `GET /api/questions` List all questions (auth required)
- `GET /api/questions/{id}` Get question by id (auth required)
- `POST /api/questions` Create question (auth required)
- `PUT /api/questions/{id}` Update question (auth required)
- `DELETE /api/questions/{id}` Delete question (auth required)
- `GET /api/questions/search/{searchString}`Search questions (auth required)

### Export
- `GET /api/exports` List all plugins available
- `GET /api/questions/{quizId}/{pluginName}` Exectute plugin to export quiz

## CI/CD
- GitHub Actions workflow: `.github/workflows/ci-cd.yml`
- Build, test

## Seeding Data
- On first run, the database is seeded with sample questions from `SeedData.cs` if empty.

## Security Notes
- Passwords are hashed (see `PasswordHelper`). For production, use a strong key and secure password hashing.
- JWT is used for authentication. Protect your `SecretKey`.

## Known limitations and possible upgrades
- TestContainers could have been used for Postgresql instead of in-memory database
