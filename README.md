PRD :-
1. Business Problem
Students need a centralized online learning platform where they can browse available courses, enroll in courses, access course lessons, and track their learning progress.
2. Features
Feature 1: Enroll in Course
Allow authenticated students to enroll in available courses and access their lessons.
Feature 2: Track Lesson Progress
Allow enrolled students to mark lessons as completed and track their progress within each course.
3. Actors
Student
Browse available courses.
Enroll in courses.
Access course lessons.
Mark lessons as completed.
Track course progress.
Instructor
Create and manage courses.
Add and manage course lessons.
Admin
Manage users.
Manage courses and platform data.
4. Business Rules
Enrollment Rules
Only authenticated students can enroll in courses.
The course must exist before enrollment.
The course must be available for enrollment.
Lesson Progress Rules
Only students enrolled in a course can access its lessons.
A student can mark a lesson as completed only if the lesson belongs to the enrolled course.
5. Data Model
Student
Course
Lesson
LessonProgress
6. API Endpoints
Course Endpoints
GET /api/courses
Retrieve all available courses.
GET /api/courses/{id}
Retrieve a specific course and its details.
Enrollment Endpoint
POST /api/courses/{courseId}/enroll
Enroll the authenticated student in a course.
Progress Endpoints
GET /api/courses/{courseId}/progress
Retrieve the authenticated student's progress in a specific course.
POST /api/lessons/{lessonId}/complete
Mark a lesson as completed for the authenticated student.
7. Acceptance Criteria
Feature 1: Enroll in Course
AC-01:
Given an authenticated student and an available course, when the student enrolls in the course, then a new enrollment should be created successfully.
AC-02:
Given a student is already enrolled in a course, when the student tries to enroll again, then the request should be rejected.
AC-03:
Given the requested course does not exist, when the student tries to enroll, then the API should return 404 Not Found.
AC-04:
Given an unauthenticated user, when they try to enroll in a course, then the API should return 401 Unauthorized.
Feature 2: Track Lesson Progress
AC-05:
Given a student is enrolled in a course, when the student completes a lesson, then a LessonProgress record should be created or updated successfully.
AC-06:
Given a student is not enrolled in the course, when they try to complete one of its lessons, then the request should be rejected.

Features
User Authentication & Authorization
Course Management
Lesson Management
Student Enrollment
Lesson Progress Tracking
Quizzes & Questions
Quiz Submissions
Certificates
DTOs & Model Validation
RESTful API Endpoints
Entity Framework Core
SQL Server
Clean Architecture
Actors
Student
Browse available courses
Enroll in courses
Access lessons
Track learning progress
Take quizzes
View quiz results
Obtain certificates
Instructor
Create and manage courses
Add and manage lessons
Create quizzes and questions
Monitor course content
Admin
Manage users
Manage courses and platform data
Control system access and permissions
Architecture
The project follows Clean Architecture principles and is divided into four main layers:
OnlineLearningPlatform
│
├── Domain
│   ├── Entities
│   ├── Enums
│   └── Interfaces
│
├── Application
│   ├── DTOs
│   ├── Interfaces
│   ├── Services
│   └── Business Logic
│
├── Infrastructure
│   ├── Data
│   ├── Repositories
│   ├── Entity Configurations
│   └── External Services
│
└── API
    ├── Controllers
    ├── Middleware
    └── Program.cs
Technologies
C#
ASP.NET Core Web API
Entity Framework Core
SQL Server
LINQ
RESTful APIs
JWT Authentication
Dependency Injection
Clean Architecture
Repository Pattern
DTO Pattern
Main Entities
Student
Instructor
Course
Lesson
Enrollment
Lesson Progress
Quiz
Question
Answer
Quiz Submission
Certificate
API
The application exposes RESTful API endpoints for managing users, courses, lessons, enrollments, quizzes, progress, and certificates.
Example:
GET /api/courses
GET /api/courses/{id}
POST /api/courses
PUT /api/courses/{id}
DELETE /api/courses/{id}
Validation & Security
JWT-based authentication
Role-based authorization
Model validation
Protected endpoints
Business rule validation
Database
The project uses SQL Server as the database and Entity Framework Core for data access and database management.
Project Goal
The goal of this project is to build a scalable and maintainable backend for an online learning platform while applying real-world backend development concepts and software architecture principles.
