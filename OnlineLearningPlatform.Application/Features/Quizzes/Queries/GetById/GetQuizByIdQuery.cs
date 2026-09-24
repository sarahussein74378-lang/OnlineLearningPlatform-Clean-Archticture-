using MediatR;
using OnlineLearningPlatform.Application.DTOs.Quiz;

namespace OnlineLearningPlatform.Application.Features.Quizzes.Queries.GetById;

public record GetQuizByIdQuery(int Id) : IRequest<QuizDto?>;
