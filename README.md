# InternLink 🔗
### AI-Powered Internship Matching Platform
### UI Link: [https://github.com/nohaa3mr/internlink-ui]
 
InternLink is a Minimal API that intelligently matches students to internships based on skill compatibility using Google Gemini. Built with Clean Architecture, CQRS, and JWT authentication.
 
---
 
##  Features
 
- AI-powered skill matching between students and internships
- JWT authentication with refresh tokens
- Role-based access (Student / Recruiter)
- Secure credential storage with PBKDF2 + HMAC-SHA256
- Structured logging with Serilog
- CQRS pattern with MediatR
- Generic repository pattern over EF Core
---
 
##  Tech Stack
 
| Layer | Technology |
|---|---|
| Framework | ASP.NET Core 10 Web API |
| Architecture | Clean Architecture + CQRS |
| Database | SQL Server + Entity Framework Core 8 |
| Authentication | JWT Bearer + Refresh Tokens |
| AI Matching | Google Gemini  |
| Mapping | Mapster |
| Validation | FluentValidation |
| Logging | Serilog |
| Documentation | Swagger UI + Scalar |
 
---
 
##  Project Structure
 
```
InternLink/
├── InternLink.API/              # Controllers, ViewModels, Middleware
├── InternLink.Application/      # Commands, Queries, DTOs, Handlers
├── InternLink.Domain/           # Entities, Interfaces
└── InternLink.Infrastructure/   # EF Core, Repositories, External Services
```
 
---
 
## ⚙️ Getting Started
 
### Prerequisites
 
- .NET 10 SDK
- SQL Server
- Google Gemini API Key
## 📡 API Endpoints
 
### Auth
| Method | Endpoint | Description | Auth |
|---|---|---|---|
| POST | `/api/auth/registration` | Register a new user | ❌ |
| POST | `/api/auth/login` | Login and get JWT token | ❌ |
| POST | `/api/auth/refresh` | Refresh access token | ❌ |
 
### Recruiter
| Method | Endpoint | Description | Auth |
|---|---|---|---|
| POST | `/api/recruiter/create-profile` | Create recruiter profile | ✅ |
| POST | `/api/internships` | Post a new internship | ✅ |
| GET | `/api/internships` | Get all internships | ✅ |
 
### Student
| Method | Endpoint | Description | Auth |
|---|---|---|---|
| POST | `/api/student/create-profile` | Create student profile | ✅ |
| POST | `/api/student/apply` | Apply to an internship (AI matching) | ✅ |
 
---
 
##  Authentication
 
InternLink uses JWT Bearer tokens. To authenticate in Swagger:
 
1. Register or login to get a token
2. Click **Authorize** in Swagger UI
3. Enter: `Bearer your_token_here`
---
 
##  AI Matching
 
When a student applies to an internship, InternLink uses **Claude AI** to:
 
1. Compare the student's skills against the internship requirements
2. Generate a match score
3. Store the result with the application
---
 
##  Patterns Used
 
| Pattern | Usage |
|---|---|
| Clean Architecture | Separation of API / Application / Domain / Infrastructure |
| CQRS | Commands and Queries via MediatR |
| Repository Pattern | `IGeneralRepository<T>` abstraction over EF Core |
| Orchestrator Pattern | Coordinates AI + DB operations for student applications |
| Result Pattern | `Result<T>` for consistent API responses |
 
---
 
##  Response Structure
 
All endpoints return a consistent `Result<T>` wrapper:
 
```json
{
  "IsSuccess": true,
  "error": null,
  "Data": { }
}
```
 
```json
{
  "IsSuccess": false,
  "error": "Recruiter profile not found",
  "Data": null
}
```
