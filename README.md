# KvizAPI

A .NET 8 Web API for managing quizzes, questions, and user authentication with JWT.

## Features
- User registration and login with JWT authentication
- CRUD operations for quizzes and questions
- Global query filters for soft-delete
- Entity Framework Core with PostgreSQL
- Swagger UI with JWT Authorize support
- Error handling middleware
- Seed data for questions
- CI/CD pipeline with build/test steps

## Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [PostgreSQL](https://www.postgresql.org/)

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
- `POST /api/auth/register` — Register a new user
- `POST /api/auth/login` — Login and get JWT

### Quizzes
- `GET /api/quiz` — List all quizzes (auth required)
- `POST /api/quiz` — Create quiz (auth required)
- `PUT /api/quiz/{id}` — Update quiz (auth required)
- `DELETE /api/quiz/{id}` — Delete quiz (auth required)

### Questions
- `GET /api/questions` — List all questions (auth required)
- `GET /api/questions/{id}` — Get question by id (auth required)
- `POST /api/questions` — Create question (auth required)
- `PUT /api/questions/{id}` — Update question (auth required)
- `DELETE /api/questions/{id}` — Delete question (auth required)
- `GET /api/questions/search/{searchString}` — Search questions (auth required)

## CI/CD
- GitHub Actions workflow: `.github/workflows/ci-cd.yml`
- Build, test

## Seeding Data
- On first run, the database is seeded with sample questions from `SeedData.cs` if empty.

## Security Notes
- Passwords are hashed (see `PasswordHelper`). For production, use a strong key and secure password hashing.
- JWT is used for authentication. Protect your `SecretKey`.

