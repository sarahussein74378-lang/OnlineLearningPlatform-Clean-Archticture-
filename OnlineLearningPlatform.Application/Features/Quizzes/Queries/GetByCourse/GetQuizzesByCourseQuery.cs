using MediatR;
using OnlineLearningPlatform.Application.DTOs.Quiz;
using System.Collections.Generic;

namespace OnlineLearningPlatform.Application.Features.Quizzes.Queries.GetByCourse;

public record GetQuizzesByCourseQuery(int CourseId) : IRequest<IEnumerable<QuizDto>>;
