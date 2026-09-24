using MediatR;
using OnlineLearningPlatform.Application.DTOs.Quiz;

namespace OnlineLearningPlatform.Application.Features.Quizzes.Commands.CreateQuiz;

public record CreateQuizCommand(CreateQuizDto Dto) : IRequest<QuizDto>;
