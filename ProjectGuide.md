# Online Learning Platform API — Step-by-Step Build Guide

## Overview

A full-featured ASP.NET Core 8 Web API using:
- **Architecture:** Three-tier (API → BLL → DAL)
- **Database:** EF Core 8 + SQL Server
- **Auth:** ASP.NET Core Identity + JWT Bearer (roles: Admin, Instructor, Student)
- **Patterns:** Generic Repository + Unit of Work
- **Mapping:** AutoMapper 13
- **Validation:** FluentValidation 11
- **Response:** Standardized `ApiResponse<T>` wrapper
- **PDF:** QuestPDF (community license)

---

## Step 1: Create the Solution and Projects

```bash
dotnet new sln -n OnlineLearningPlatform

dotnet new classlib -n OnlineLearningPlatform.DAL -f net8.0
dotnet new classlib -n OnlineLearningPlatform.BLL -f net8.0
dotnet new webapi  -n OnlineLearningPlatform.API  -f net8.0 --no-openapi

dotnet sln add OnlineLearningPlatform.DAL/OnlineLearningPlatform.DAL.csproj
dotnet sln add OnlineLearningPlatform.BLL/OnlineLearningPlatform.BLL.csproj
dotnet sln add OnlineLearningPlatform.API/OnlineLearningPlatform.API.csproj
```

### Add project references

```bash
# API depends on BLL
dotnet add OnlineLearningPlatform.API/OnlineLearningPlatform.API.csproj reference OnlineLearningPlatform.BLL/OnlineLearningPlatform.BLL.csproj

# BLL depends on DAL
dotnet add OnlineLearningPlatform.BLL/OnlineLearningPlatform.BLL.csproj reference OnlineLearningPlatform.DAL/OnlineLearningPlatform.DAL.csproj
```

---

## Step 2: Install NuGet Packages

### DAL packages

```bash
dotnet add OnlineLearningPlatform.DAL/OnlineLearningPlatform.DAL.csproj package Microsoft.EntityFrameworkCore --version 8.0.0
dotnet add OnlineLearningPlatform.DAL/OnlineLearningPlatform.DAL.csproj package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.0
dotnet add OnlineLearningPlatform.DAL/OnlineLearningPlatform.DAL.csproj package Microsoft.EntityFrameworkCore.Tools --version 8.0.0
dotnet add OnlineLearningPlatform.DAL/OnlineLearningPlatform.DAL.csproj package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 8.0.0
```

### BLL packages

```bash
dotnet add OnlineLearningPlatform.BLL/OnlineLearningPlatform.BLL.csproj package AutoMapper --version 13.0.1
dotnet add OnlineLearningPlatform.BLL/OnlineLearningPlatform.BLL.csproj package FluentValidation --version 11.9.0
dotnet add OnlineLearningPlatform.BLL/OnlineLearningPlatform.BLL.csproj package FluentValidation.DependencyInjectionExtensions --version 11.9.0
dotnet add OnlineLearningPlatform.BLL/OnlineLearningPlatform.BLL.csproj package QuestPDF --version 2024.3.4
dotnet add OnlineLearningPlatform.BLL/OnlineLearningPlatform.BLL.csproj package Microsoft.Extensions.Configuration.Abstractions --version 8.0.0
dotnet add OnlineLearningPlatform.BLL/OnlineLearningPlatform.BLL.csproj package Microsoft.Extensions.Identity.Core --version 8.0.0
dotnet add OnlineLearningPlatform.BLL/OnlineLearningPlatform.BLL.csproj package System.IdentityModel.Tokens.Jwt --version 7.3.1
dotnet add OnlineLearningPlatform.BLL/OnlineLearningPlatform.BLL.csproj package Microsoft.IdentityModel.Tokens --version 7.3.1
```

### API packages

```bash
dotnet add OnlineLearningPlatform.API/OnlineLearningPlatform.API.csproj package Microsoft.AspNetCore.Authentication.JwtBearer --version 8.0.0
dotnet add OnlineLearningPlatform.API/OnlineLearningPlatform.API.csproj package Swashbuckle.AspNetCore --version 6.5.0
dotnet add OnlineLearningPlatform.API/OnlineLearningPlatform.API.csproj package QuestPDF --version 2024.3.4
dotnet add OnlineLearningPlatform.API/OnlineLearningPlatform.API.csproj package Microsoft.EntityFrameworkCore.Design --version 8.0.0
```

---

## Step 3: DAL — Entities

Create all entity classes inside `OnlineLearningPlatform.DAL/Entities/`.

### Entity list

| File | Key Properties |
|------|---------------|
| `ApplicationUser.cs` | Extends `IdentityUser` — FirstName, LastName, ProfilePictureUrl |
| `InstructorProfile.cs` | UserId (FK), Bio, Expertise, Rating, → Courses |
| `StudentProfile.cs` | UserId (FK), → Enrollments, Certificates |
| `Course.cs` | Title, Description, Price, Level, Category, IsPublished, → Lessons, Enrollments, Quizzes |
| `Lesson.cs` | Title, VideoUrl, Content, OrderIndex, DurationMinutes, IsFreePreview, → CourseId |
| `Enrollment.cs` | StudentProfileId, CourseId, Status (enum), → LessonProgresses, Certificate |
| `LessonProgress.cs` | EnrollmentId, LessonId, IsCompleted, WatchedSeconds |
| `Quiz.cs` | Title, CourseId, PassingScore, TimeLimitMinutes, → Questions, Submissions |
| `Question.cs` | Text, Points, OrderIndex, → Answers |
| `Answer.cs` | Text, IsCorrect, → QuestionId |
| `QuizSubmission.cs` | QuizId, StudentProfileId, Score, TotalPoints, IsPassed |
| `QuizSubmissionAnswer.cs` | QuizSubmissionId, QuestionId, SelectedAnswerId, IsCorrect |
| `Certificate.cs` | StudentProfileId, EnrollmentId, CertificateNumber, IssuedAt, PdfPath |

### EnrollmentStatus enum (in `Enrollment.cs`)

```csharp
public enum EnrollmentStatus { Active, Completed, Dropped }
```

---

## Step 4: DAL — EF Configurations

Create `OnlineLearningPlatform.DAL/Data/Configurations/` and add one `IEntityTypeConfiguration<T>` class per entity that needs config.

### Key configuration rules

| Configuration | Rule |
|---------------|------|
| `CourseConfiguration` | Price → `decimal(18,2)`, Title max 200, Instructor FK → `Restrict` |
| `InstructorProfileConfiguration` | Rating → `decimal(3,2)`, User FK → `Cascade` |
| `EnrollmentConfiguration` | Unique index on (StudentProfileId, CourseId), Student/Course FK → `Restrict` |
| `LessonProgressConfiguration` | Unique index on (EnrollmentId, LessonId), Lesson FK → **`NoAction`** |
| `QuizConfiguration` | Course FK → `Cascade` |
| `QuizSubmissionAnswerConfiguration` | Question FK → **`NoAction`**, SelectedAnswer FK → **`NoAction`** |
| `CertificateConfiguration` | Unique index on CertificateNumber |

> **Important:** `NoAction` on `LessonProgress.LessonId` and `QuizSubmissionAnswer.QuestionId / SelectedAnswerId` is required to avoid SQL Server "multiple cascade paths" errors.

---

## Step 5: DAL — AppDbContext

Create `OnlineLearningPlatform.DAL/Data/AppDbContext.cs`.

```csharp
public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // DbSet for every entity...

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(CourseConfiguration).Assembly);
    }
}
```

---

## Step 6: DAL — Generic Repository & Unit of Work

### `IGenericRepository<T>` interface (`Repositories/Interfaces/`)

Methods: `GetByIdAsync`, `GetAllAsync`, `FindAsync`, `FindFirstAsync`, `AddAsync`, `Update`, `Delete`, `ExistsAsync`, `Query` (returns `IQueryable<T>`).

### `GenericRepository<T>` implementation (`Repositories/Implementations/`)

Wraps `DbSet<T>` from the injected `AppDbContext`.

### `IUnitOfWork` interface (`UnitOfWork/`)

One `IGenericRepository<T>` property per entity + `SaveChangesAsync()`.

### `UnitOfWork` implementation

Initializes all repositories in the constructor, delegates `SaveChangesAsync` to `_context.SaveChangesAsync()`.

---

## Step 7: BLL — Common Types

Create `OnlineLearningPlatform.BLL/Common/`:

**`ApiResponse<T>`** — wraps all API responses:
```csharp
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public T? Data { get; set; }
    public IEnumerable<string>? Errors { get; set; }

    public static ApiResponse<T> Ok(T data, string message = "Success") => ...
    public static ApiResponse<T> Fail(string message, ...) => ...
}
```

**`PaginatedResult<T>`** — for paginated lists: Items, TotalCount, Page, PageSize, TotalPages, HasNextPage, HasPreviousPage.

---

## Step 8: BLL — DTOs

Create `OnlineLearningPlatform.BLL/DTOs/` with subfolders per feature:

| Folder | DTOs |
|--------|------|
| `Auth/` | `RegisterDto`, `LoginDto`, `AuthResultDto` |
| `Course/` | `CreateCourseDto`, `UpdateCourseDto`, `CourseDto` |
| `Lesson/` | `CreateLessonDto`, `LessonDto` |
| `Enrollment/` | `EnrollmentDto` |
| `Progress/` | `ProgressDto`, `LessonProgressDto`, `UpdateLessonProgressDto` |
| `Quiz/` | `CreateQuizDto`, `CreateQuestionDto`, `CreateAnswerDto`, `QuizDto`, `QuestionDto`, `AnswerDto`, `SubmitQuizDto`, `QuizResultDto`, `QuestionResultDto` |
| `Certificate/` | `CertificateDto` |

> **Note:** `AnswerDto` does NOT expose `IsCorrect` — correct answers must never be sent to the client before submission.

---

## Step 9: BLL — AutoMapper Profile

Create `OnlineLearningPlatform.BLL/Mapping/MappingProfile.cs` extending `Profile`.

Key mappings:
- `Course → CourseDto`: map `InstructorName` from `Instructor.User.FirstName + LastName`, `TotalLessons` from `Lessons.Count`
- `Enrollment → EnrollmentDto`: map `CourseTitle`, ignore `ProgressPercentage` (calculated in service)
- `Certificate → CertificateDto`: map student name and instructor name through navigations

---

## Step 10: BLL — FluentValidation Validators

Create `OnlineLearningPlatform.BLL/Validators/`:

| Validator | Rules |
|-----------|-------|
| `RegisterValidator` | Email format, password min 6 + uppercase + digit, Role must be "Instructor" or "Student" |
| `CreateCourseValidator` | Title max 200, Price ≥ 0, Level must be Beginner/Intermediate/Advanced |
| `CreateLessonValidator` | Title max 300, CourseId > 0 |
| `CreateQuizValidator` | At least 1 question, each question ≥ 2 answers, exactly 1 correct answer per question |

---

## Step 11: BLL — Service Interfaces

Create `OnlineLearningPlatform.BLL/Services/Interfaces/`:

| Interface | Key Methods |
|-----------|-------------|
| `IAuthService` | `RegisterAsync(RegisterDto)`, `LoginAsync(LoginDto)` |
| `ICourseService` | `GetAllAsync(page, pageSize, category, level)`, `CreateAsync`, `UpdateAsync`, `DeleteAsync` |
| `ILessonService` | `GetByCourseAsync`, `CreateAsync`, `UpdateAsync`, `DeleteAsync` |
| `IEnrollmentService` | `EnrollAsync`, `GetStudentEnrollmentsAsync`, `DropCourseAsync` |
| `IProgressService` | `GetProgressAsync`, `UpdateLessonProgressAsync` |
| `IQuizService` | `CreateAsync`, `GetByCourseAsync`, `SubmitAsync` (grades and saves result) |
| `ICertificateService` | `IssueCertificateAsync`, `GetStudentCertificatesAsync`, `GeneratePdfAsync` |

---

## Step 12: BLL — Service Implementations

Create `OnlineLearningPlatform.BLL/Services/Implementations/`.

### AuthService
- Uses `UserManager<ApplicationUser>` for register/login
- On register: creates `InstructorProfile` or `StudentProfile` based on role
- JWT token built with `JwtSecurityTokenHandler`, claims include `NameIdentifier`, `Email`, `Role`

### CourseService
- Uses `IQueryable` + `Include` for eager loading, `Skip/Take` for pagination
- `UpdateAsync` verifies the calling instructor owns the course

### ProgressService
- `UpdateLessonProgressAsync`: creates or updates `LessonProgress`
- After saving, calls `CheckAndCompleteCourseAsync`: if all lessons completed → set `Enrollment.Status = Completed`

### QuizService — `SubmitAsync`
- Iterates submitted answers, checks each against `Answer.IsCorrect`
- Calculates score and percentage
- Saves `QuizSubmission` + all `QuizSubmissionAnswer` records
- Returns full `QuizResultDto` with per-question feedback

### CertificateService
- `IssueCertificateAsync`: only works if `Enrollment.Status == Completed`
- Generates a unique `CertificateNumber` = `CERT-{date}-{GUID slice}`
- Builds PDF with QuestPDF (fluent document API, A4 landscape)
- Saves PDF to disk at `wwwroot/certificates/`

---

## Step 13: API — Middleware

Create `OnlineLearningPlatform.API/Middleware/ExceptionMiddleware.cs`.

Catches all unhandled exceptions and maps them to HTTP status codes:
- `UnauthorizedAccessException` → 401
- `KeyNotFoundException` → 404
- `InvalidOperationException` → 400
- Everything else → 500

Returns a JSON `ApiResponse` with `Success = false`.

---

## Step 14: API — Service Extensions

Create `OnlineLearningPlatform.API/Extensions/ServiceExtensions.cs` with static extension methods on `IServiceCollection`:

| Method | Registers |
|--------|-----------|
| `AddDatabase` | `AppDbContext` with SQL Server connection string |
| `AddIdentityServices` | `IdentityRole`, password rules, EF stores |
| `AddJwtAuthentication` | JWT Bearer with issuer/audience/key validation |
| `AddApplicationServices` | All service interfaces → implementations (Scoped) |
| `AddMappingAndValidation` | AutoMapper profile, FluentValidation from assembly |
| `AddSwagger` | Swagger with JWT Bearer security definition |

---

## Step 15: API — Controllers

Create all controllers in `OnlineLearningPlatform.API/Controllers/`.

### Authorization summary

| Controller | Auth requirement |
|------------|-----------------|
| `AuthController` | Anonymous |
| `CoursesController` | GET: anonymous · POST/PUT/DELETE: `[Instructor]` |
| `LessonsController` | GET: anonymous · POST/PUT/DELETE: `[Instructor]` |
| `EnrollmentsController` | All: `[Student]` |
| `ProgressController` | All: `[Student]` |
| `QuizzesController` | GET: `[Authorize]` · POST/DELETE: `[Instructor]` · Submit: `[Student]` |
| `CertificatesController` | All: `[Authorize]` · Issue: `[Admin,Student]` |

### Extract the current user ID in controllers

```csharp
var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
```

---

## Step 16: API — Program.cs

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddIdentityServices();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddMappingAndValidation();
builder.Services.AddSwagger();

var app = builder.Build();

// Seed roles on startup
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    foreach (var role in new[] { "Admin", "Instructor", "Student" })
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
```

---

## Step 17: appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=OnlineLearningPlatformDb;Trusted_Connection=True;TrustServerCertificate=True"
  },
  "Jwt": {
    "Key": "YourSuperSecretKeyThatIsAtLeast32CharactersLong!",
    "Issuer": "OnlineLearningPlatformAPI",
    "Audience": "OnlineLearningPlatformClients",
    "ExpiryHours": "24"
  },
  "Certificates": {
    "StoragePath": "wwwroot/certificates"
  }
}
```

> Change the JWT Key to a strong secret before deploying to production.

---

## Step 18: EF Core Migration & Database

```bash
# Add migration
dotnet ef migrations add InitialCreate \
  --project OnlineLearningPlatform.DAL \
  --startup-project OnlineLearningPlatform.API

# Apply to database
dotnet ef database update \
  --project OnlineLearningPlatform.DAL \
  --startup-project OnlineLearningPlatform.API
```

---

## Step 19: Run the API

```bash
dotnet run --project OnlineLearningPlatform.API
```

Open Swagger UI at: `https://localhost:{port}/swagger`

Roles (Admin, Instructor, Student) are created automatically on the first run.

---

## Final Project Structure

```
OnlineLearningPlatform.slnx
│
├── OnlineLearningPlatform.DAL/
│   ├── Entities/               ApplicationUser, Course, Lesson, Enrollment,
│   │                           LessonProgress, Quiz, Question, Answer,
│   │                           QuizSubmission, QuizSubmissionAnswer, Certificate
│   ├── Data/
│   │   ├── AppDbContext.cs
│   │   └── Configurations/     One IEntityTypeConfiguration<T> per entity
│   ├── Repositories/
│   │   ├── Interfaces/         IGenericRepository<T>
│   │   └── Implementations/    GenericRepository<T>
│   └── UnitOfWork/             IUnitOfWork, UnitOfWork
│
├── OnlineLearningPlatform.BLL/
│   ├── Common/                 ApiResponse<T>, PaginatedResult<T>
│   ├── DTOs/                   Auth/, Course/, Lesson/, Enrollment/,
│   │                           Progress/, Quiz/, Certificate/
│   ├── Mapping/                MappingProfile.cs
│   ├── Validators/             RegisterValidator, CreateCourseValidator,
│   │                           CreateLessonValidator, CreateQuizValidator
│   └── Services/
│       ├── Interfaces/         IAuthService, ICourseService, ILessonService,
│       │                       IEnrollmentService, IProgressService,
│       │                       IQuizService, ICertificateService
│       └── Implementations/    One class per interface
│
└── OnlineLearningPlatform.API/
    ├── Controllers/            Auth, Courses, Lessons, Enrollments,
    │                           Progress, Quizzes, Certificates
    ├── Middleware/             ExceptionMiddleware.cs
    ├── Extensions/             ServiceExtensions.cs
    ├── Program.cs
    └── appsettings.json
```

---

## Common Issues & Fixes

### "Multiple cascade paths" error on migration apply
SQL Server rejects cascades that form a cycle. Fix by setting affected FKs to `NoAction` in EF configuration:
- `LessonProgress.LessonId` → `NoAction`
- `QuizSubmissionAnswer.QuestionId` → `NoAction`
- `QuizSubmissionAnswer.SelectedAnswerId` → `NoAction`

### Private NuGet feed 401 errors
If your machine has a private NuGet source configured, use `--source https://api.nuget.org/v3/index.json` on each `dotnet add package` command to target nuget.org directly.

### EF Tools not found
Install globally: `dotnet tool install --global dotnet-ef`
