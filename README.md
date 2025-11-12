# QuestBoard API

A Trello-style backend for team collaboration and task management.

**Stack:** ASP.NET Core · EF Core · JWT · Serilog · AutoMapper · SQLite/SQL Server

## Features
- JWT auth with RBAC policies (Admin, Manager, Member)
- CRUD endpoints for projects, tasks, and comments
- Task assignment workflow with background email queue
- File upload support with static serving
- Filtering, pagination, and soft deletes with audit metadata
- AutoMapper DTO mapping and Serilog structured logging

## Getting Started
```bash
git clone https://github.com/Semir-Harun/QuestBoard-API.git
cd QuestBoard-API
dotnet restore

# Configure secrets (JWT key & SMTP credentials)
dotnet user-secrets init --project QuestBoard.Api
dotnet user-secrets set "Jwt:Key" "<dev-secret>" --project QuestBoard.Api

# Apply database migrations
dotnet ef database update --project QuestBoard.Infrastructure --startup-project QuestBoard.Api

# Launch the API
dotnet run --project QuestBoard.Api
```

Open https://localhost:5001/swagger once the app is running.

> **Heads-up:** the project targets .NET 6. Install the .NET 6 SDK/runtime locally or use the Docker workflow below if you only have newer runtimes installed.

> ⚙️ **Prerequisite**  
> Install [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0) or run inside Docker.

### 🪄 Local Setup
```bash
git clone https://github.com/<you>/QuestBoard-API.git
cd QuestBoard-API
dotnet restore

# Configure secrets (JWT key & SMTP credentials)
dotnet user-secrets init --project QuestBoard.Api
dotnet user-secrets set "Jwt:Key" "<dev-secret>" --project QuestBoard.Api

# Apply database migrations
dotnet ef database update --project QuestBoard.Infrastructure --startup-project QuestBoard.Api

# Launch the API
dotnet run --project QuestBoard.Api
```

Open https://localhost:5001/swagger to explore the API.

## Solution Layout
- `QuestBoard.Api` – Presentation layer hosting controllers, filters, and swagger setup
- `QuestBoard.Application` – Application services, DTOs, policies, and abstractions
- `QuestBoard.Domain` – Core entities, enums, and domain logic
- `QuestBoard.Infrastructure` – EF Core persistence, repositories, auth, files, email background jobs
- `QuestBoard.Tests` – xUnit test suites for API/Application/Infrastructure

## CI
A sample GitHub Actions workflow is located in `.github/workflows/ci.yml` (add after initializing the repo on GitHub).

## License
Distributed under the MIT License. See `LICENSE` for details.
